namespace ExtensionMethods
{
    public static class Extensions
    {
        public static float ToTime(this int frames)
        {
            return frames / (float)Player.AttackTimer.FPS;
        }
    }
}