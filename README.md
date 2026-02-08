# Documentation — Projet Bibliotheque

## Résumé rapide
- Les fichiers utilisateur / livres sont sauvegardés chiffrés (AES-256) si on fournit une clé.
- Si l'utilisateur ne donne pas de clé, le programme utilise le SID Windows courant comme clé dérivée.
- Format de fichier chiffré : header `ENCF` + longueur du sel + sel, puis données AES-CBC+PKCS7.
- Lors du chargement, si la décryption échoue, l'application propose à l'utilisateur de réessayer.
- Les erreurs de décryptage sont catégorisées : `FileNotFound`, `InvalidFormat`, `WrongKeyOrCorrupt`, `OtherError`.
- Lecture (login) n'entraîne pas la création automatique du dossier utilisateur — création uniquement à la sauvegarde.

## Fichiers importants (où regarder)
- `SerializationApp/CryptoHelper.cs`  
  - Contient la logique AES + PBKDF2. Fonctions utiles :
    - `EncryptStreamToFile(Stream, path, password)` — chiffre et écrit le fichier.
    - `TryDecryptFileToMemoryStream(path, password, out MemoryStream, out string)` — tente de déchiffrer et retourne un code d'erreur si échec.
    - `EffectivePassword(password)` — si `password` vide, retourne le SID Windows.
- `SerializationApp/EncryptedXmlSerializer.cs` et `SerializationApp/EncryptedBinarySerializer.cs`  
  - Sérialisent en mémoire puis utilisent `CryptoHelper` pour écrire/charger.
  - Fournissent `TryLoad<T>(...)` qui retourne une erreur lisible.
- `App/UserRepository.cs`  
  - Expose `SaveProfile(..., key)` / `LoadProfile(key)` et `SaveBooks` / `LoadBooks` acceptant la clé.
  - Si le fichier existe mais ne peut pas être décrypté, une `DecryptionException` est levée pour que l'appelant (UI/Program) propose de réessayer.
- `App/Program.cs`  
  - Flux console : à la création / login / save / load, l'utilisateur est invité à fournir une clé. S'il y a une erreur de décryptage on propose de réessayer.
- `App/PathManager.cs`  
  - `GetUserFolderPath(username)` retourne le chemin SANS créer le dossier (utile pour tests / suppression).
  - `GetUserFolder(username)` crée le dossier (utilisé avant sauvegarde).
- `App/Diagnostics.cs`  
  - Outils de debug pour vérifier droits, écriture, ACL, etc.

## Utilisation côté utilisateur (console)
- Créer un compte :
  - Le programme demande un username / mot de passe / email ... puis demande une clé de chiffrement (optionnelle).
  - Si la clé est vide, le SID Windows est utilisé automatiquement.
- Se connecter :
  - On entre username + mot de passe puis la clé (vide utilisera SID).
  - Si le fichier de profil existe mais la clé est incorrecte, on est invité à réessayer ou annuler.
- Sauvegarder / Charger livres :
  - Idem : l'app demande la clé pour chiffrer/déchiffrer.
  - En cas d'erreur de décryptage, on propose de retenter.

## Comportement sur erreurs
- Les erreurs de chiffrement/déchiffrement sont capturées. L'application affiche un message et propose de réessayer.
- Si on choisit de ne pas réessayer lors d'un login, l'utilisateur est traité comme "inconnu".
- Les exceptions non liées à la cryptographie (ex: IO, permission) sont remontées et affichées lors des opérations de sauvegarde.

## Remarques pédagogiques / sécurité
- Les clés sont lues en clair depuis la console.
- Le choix du SID comme clé par défaut est pratique pour uni-utilisateur sur la même machine, mais rend les fichiers lisibles par toute session ayant le même SID (donc considérer la sécurité selon le contexte).

## Propositions d'amélioration (futures)
- Masquer la saisie de la clé dans la console.
- Ajout d'un mécanisme d'index/fichier manifeste non chiffré pour savoir si un user a un profil sans tenter de décryptage.
