using System;

//Example data class, with JSONConvertable attribute
[JSONConvertable, Serializable]
public class PlayerData
{
    public string PlayerName;
    public int PlayerNumber;
}
