namespace BookMyShow.Custom_Exceptions
{
    class ScreenNotFoundException : Exception
    {
        public ScreenNotFoundException(int screenno, string theatrename) 
            : base(String.Format($"Screen number {screenno} does not exist in {theatrename}")) { }
    }
}
