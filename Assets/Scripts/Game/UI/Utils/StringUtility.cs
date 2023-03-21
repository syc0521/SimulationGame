namespace Game.UI.Utils
{
    public static class StringUtility
    {
        public static string ConvertNumberToString(int number)
        {
            var numberStr = number.ToString();
            string numStr = "0123456789";
            string chineseStr = "零一二三四五六七八九";
            char[] c = numberStr.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                int index = numStr.IndexOf(c[i]);
                if (index != -1)
                    c[i] = chineseStr.ToCharArray()[index];
            }
            return new string(c);
        } 
    }
}