public static class HelperFunctions
{
    public static int PackedIndex(int x, int y)
    {
        int plainIndex = x;
        plainIndex <<= 16;
        plainIndex += y;
        return plainIndex;
    }

}