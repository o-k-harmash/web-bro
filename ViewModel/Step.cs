using WebBro.Models;

namespace WebBro.ViewModel;


public class StepViewModel
{
    public Step? NextStep { get; set; }
    public Step CurrentStep { get; set; }
    public List<Step> StepList { get; set; }
}