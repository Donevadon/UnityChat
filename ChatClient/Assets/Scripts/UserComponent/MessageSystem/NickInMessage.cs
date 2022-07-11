
namespace UserComponent.MessageSystem
{
    public class NickInMessage : ColorText
    {
        public override string Text
        {
            get => base.Text;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    base.Text = "Nick:";
                }else if (value.Length <= 13)
                {
                    base.Text = value + ":";
                }
            }
        }
    }
}