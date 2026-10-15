using UnityEngine;

public static class ItemInstanceID
{
    private static long nextInstanceID = 0;

    public static long GetInstanceID()
    {
        return ++nextInstanceID;
    }
}
