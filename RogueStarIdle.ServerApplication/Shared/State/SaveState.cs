using Microsoft.JSInterop;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class SaveState
    {
        public ActionState ActionState { get; set; }
        public CharacterState CharacterState { get; set; }
        public InventoryState InventoryState { get; set; }
        public TimeState TimeState { get; set; }

        public SaveState(ActionState actionState, CharacterState characterState, InventoryState inventoryState, TimeState timeState) { 
            ActionState = actionState;
            CharacterState = characterState;
            InventoryState = inventoryState;
            TimeState = timeState;
        }

        public SaveState() {}

        private string key = "Rogue Star Baybeeeee";

        public async Task Save(Func<string, string, Task> SaveToLocalStorage)
        {
            SaveState saveState = new SaveState(ActionState, CharacterState, InventoryState, TimeState);
            string save = Serialize(saveState);
            byte[] encryptedSave = await EncryptAsync(save, key);
            string finalSave = Serialize(encryptedSave);
            await SaveToLocalStorage("saveState", finalSave);
        }

        public async Task<string> Export()
        {
            SaveState saveState = new SaveState(ActionState, CharacterState, InventoryState, TimeState);
            string save = Serialize(saveState);
            byte[] encryptedSave = await EncryptAsync(save, key);
            return Serialize(encryptedSave);
        }

        public async Task Load(Func<string, Task<string>> LoadFromLocalStorage)
        {
            SaveState? loadedState = default;
            try
            {
                string encryptedLoadString = await LoadFromLocalStorage("saveState");
                byte[] encryptedLoadBytes = Deserialize<byte[]>(encryptedLoadString);
                string decryptedLoad = await DecryptAsync(encryptedLoadBytes, key);
                loadedState = Deserialize<SaveState>(decryptedLoad);

            } catch
            {
                Console.WriteLine("Error on game Load");
            }
            if (loadedState == default(SaveState))
            {
                return;
            }
            StateCopy(ActionState, loadedState.ActionState);
            StateCopy(CharacterState, loadedState.CharacterState);
            StateCopy(InventoryState, loadedState.InventoryState);
            StateCopy(TimeState, loadedState.TimeState);
        }

        public async void Import(string importExport)
        {
            byte[] encryptedLoadBytes = Deserialize<byte[]>(importExport);
            string decryptedLoad = await DecryptAsync(encryptedLoadBytes, key);
            SaveState loadedState = Deserialize<SaveState>(decryptedLoad);
            if (loadedState == default(SaveState))
            {
                return;
            }
            StateCopy(ActionState, loadedState.ActionState);
            StateCopy(CharacterState, loadedState.CharacterState);
            StateCopy(InventoryState, loadedState.InventoryState);
            StateCopy(TimeState, loadedState.TimeState);
        }

        public async void ClearSave(Func<string, Task> DeleteFromLocalStorage)
        {
            await DeleteFromLocalStorage("saveState");
        }

        private string Serialize<T>(T state)
        {
            JsonSerializerOptions options = new JsonSerializerOptions { IncludeFields = true };
            string save = JsonSerializer.Serialize(state, options);
            return save;
        }

        private static T? Deserialize<T>(string? json)
        {
            if (json == null || json == "")
            {
                return default(T);
            }
            JsonSerializerOptions options = new JsonSerializerOptions { IncludeFields = true };
            T? state = JsonSerializer.Deserialize<T>(json, options);
            return state;
        }

        private void StateCopy<T>(T state, T loadedState)
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                if (!property.CanRead || !property.CanWrite)
                {
                    continue;
                }
                object value = property.GetValue(loadedState);
                property.SetValue(state, value);
            }
        }

        /// <summary>
        /// Encryption copypasta'd from https://code-maze.com/csharp-string-encryption-decryption/
        /// </summary>

        private byte[] IV =
        {
    0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
    0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
    };

        public async Task<byte[]> EncryptAsync(string clearText, string passphrase)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(passphrase);
            aes.IV = IV;
            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using (DeflateStream compressionStream = new(cryptoStream, CompressionMode.Compress))
            {
                await compressionStream.WriteAsync(Encoding.UTF8.GetBytes(clearText));
            }
            return output.ToArray();
        }

        public async Task<string> DecryptAsync(byte[] encrypted, string passphrase)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(passphrase);
            aes.IV = IV;
            using MemoryStream input = new(encrypted);
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using (DeflateStream decompressionStream = new(cryptoStream, CompressionMode.Decompress))
            {
                using MemoryStream output = new();
                await decompressionStream.CopyToAsync(output);
                return Encoding.UTF8.GetString(output.ToArray());
            }
        }

        private byte[] DeriveKeyFromPassword(string password)
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 16; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                             emptySalt,
                                             iterations,
                                             hashMethod,
                                             desiredKeyLength);
        }

    }
}
