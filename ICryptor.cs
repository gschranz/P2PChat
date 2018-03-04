namespace P2PChat
{
    public interface ICryptor
    {
        string Encrypt(string plaintext);

        string Decrypt(string encmsg);
    }
}