namespace Tea.Core.Domain
{
    public class RoundDetail : BaseDomain
    {
       public virtual RoundUser RoundBy { get; set; }
       public string RoundNotes { get; set; }
       public virtual Round Round { get; set; }
    }
}
