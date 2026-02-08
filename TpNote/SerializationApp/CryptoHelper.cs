using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SerializationApp
{
    internal static class CryptoHelper
    {
        // Magic header to identify encrypted files
        private static readonly byte[] Magic = Encoding.ASCII.GetBytes("ENCF"); // 4 bytes
        private const int SaltSize = 16;
        private const int KeySizeBytes = 32; // AES-256
        private const int IvSizeBytes = 16;
        private const int Iterations = 100000;

        // Returns effective password (use Windows SID when empty)
        public static string EffectivePassword(string password)
        {
            if (!string.IsNullOrEmpty(password)) return password;
            try
            {
                var wi = System.Security.Principal.WindowsIdentity.GetCurrent();
                return wi?.User?.Value ?? Environment.MachineName;
            }
            catch
            {
                return Environment.MachineName;
            }
        }

        public static void EncryptStreamToFile(Stream plainStream, string filePath, string password)
        {
            if (plainStream == null) throw new ArgumentNullException(nameof(plainStream));
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            password = EffectivePassword(password);

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            // derive key+iv
            using (var pdb = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var key = pdb.GetBytes(KeySizeBytes);
                var iv = pdb.GetBytes(IvSizeBytes);

                using (var aes = Aes.Create())
                {
                    aes.KeySize = KeySizeBytes * 8;
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    var dir = Path.GetDirectoryName(filePath);
                    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // write header: magic + salt length + salt
                        fs.Write(Magic, 0, Magic.Length);
                        var saltLen = BitConverter.GetBytes(salt.Length);
                        fs.Write(saltLen, 0, saltLen.Length);
                        fs.Write(salt, 0, salt.Length);

                        using (var crypto = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            plainStream.Position = 0;
                            plainStream.CopyTo(crypto);
                            crypto.FlushFinalBlock();
                        }
                    }
                }
            }
        }

        // Non?throwing try-decrypt that provides error information
        public static bool TryDecryptFileToMemoryStream(string filePath, string password, out MemoryStream result, out string error)
        {
            result = null;
            error = null;

            if (filePath == null)
            {
                error = "InvalidPath";
                return false;
            }
            if (!File.Exists(filePath))
            {
                error = "FileNotFound";
                return false;
            }

            password = EffectivePassword(password);

            try
            {
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                // We must not dispose fs here before returning the decrypted memory stream, so do reads on fs,
                // but wrap decrypt operation in try/catch and close fs afterwards.
                using (fs)
                {
                    // read and verify header
                    var header = new byte[Magic.Length];
                    if (fs.Read(header, 0, header.Length) != header.Length)
                        throw new InvalidDataException("Invalid encrypted file header.");
                    for (int i = 0; i < Magic.Length; i++)
                        if (header[i] != Magic[i]) throw new InvalidDataException("File is not in expected encrypted format.");

                    var lenBuf = new byte[4];
                    if (fs.Read(lenBuf, 0, 4) != 4) throw new InvalidDataException("Invalid encrypted file header.");
                    int saltLen = BitConverter.ToInt32(lenBuf, 0);
                    if (saltLen <= 0 || saltLen > 1024) throw new InvalidDataException("Invalid salt length in header.");

                    var salt = new byte[saltLen];
                    if (fs.Read(salt, 0, saltLen) != saltLen) throw new InvalidDataException("Invalid encrypted file header.");

                    using (var pdb = new Rfc2898DeriveBytes(password, salt, Iterations))
                    {
                        var key = pdb.GetBytes(KeySizeBytes);
                        var iv = pdb.GetBytes(IvSizeBytes);

                        using (var aes = Aes.Create())
                        {
                            aes.Key = key;
                            aes.IV = iv;
                            aes.Mode = CipherMode.CBC;
                            aes.Padding = PaddingMode.PKCS7;

                            using (var crypto = new CryptoStream(fs, aes.CreateDecryptor(), CryptoStreamMode.Read))
                            {
                                var ms = new MemoryStream();
                                crypto.CopyTo(ms);
                                ms.Position = 0;
                                result = ms;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (InvalidDataException)
            {
                error = "InvalidFormat";
                return false;
            }
            catch (CryptographicException)
            {
                // Wrong key or corrupted data (padding/crypto failure)
                error = "WrongKeyOrCorrupt";
                return false;
            }
            catch (Exception ex)
            {
                error = "OtherError: " + ex.Message;
                return false;
            }
        }

        // Backwards-compatible helper that returns null on any error (keeps older behavior)
        public static MemoryStream DecryptFileToMemoryStream(string filePath, string password)
        {
            MemoryStream ms;
            string err;
            if (TryDecryptFileToMemoryStream(filePath, password, out ms, out err))
                return ms;
            return null;
        }
    }
}