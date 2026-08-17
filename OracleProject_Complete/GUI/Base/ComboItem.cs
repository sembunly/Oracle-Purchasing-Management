namespace OracleProject
{
    /// <summary>
    /// Simple combo box item with ID and display text.
    /// </summary>
    internal sealed class ComboItem
    {
        public ComboItem(int id, string text)
        {
            Id = id;
            Text = text;
        }

        public int Id { get; private set; }
        private string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
