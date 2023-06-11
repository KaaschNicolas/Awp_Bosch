using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
namespace App.Core.DTOs;

[Keyless]
public class PcbDTO
{
    public int PcbId { get; set; }

    public string StorageName { get; set; }
    public int DwellTime { get; set; }


    public int DwellTimeStatus { get; set; }


    public string DwellTimeRed { get; set; }

    public string DwellTimeYellow { get; set; }


    private SolidColorBrush _dwellTimeColor;

    [NotMapped]
    public SolidColorBrush DwellTimeColor
    {
        get => getColor();
        set
        {
            _dwellTimeColor = value;
        }
    }

    public DateTime FailedAt { get; set; }

    public bool IsFinalized { get; set; }

    public string SerialNumber { get; set; }

    public string PcbPartNumber { get; set; }

    //public string Error { get; set; }

    public int TransferCount { get; set; }

    public PcbDTO() { }

    public SolidColorBrush getColor()
    {
        var StatusColor = new SolidColorBrush();
        if (DwellTimeRed != "--" && DwellTimeYellow != "--")
        {

            if (DwellTime >= int.Parse(DwellTimeRed))
            {

                StatusColor = new SolidColorBrush(Colors.Red);

            }
            else if (DwellTime >= int.Parse(DwellTimeYellow))
            {
                StatusColor = new SolidColorBrush(Colors.Yellow);
            }
            else
            {
                StatusColor = new SolidColorBrush(Colors.LimeGreen);

            }
            return StatusColor;
        }
        else
        {
            return new SolidColorBrush(Colors.Transparent);
        }
    }
}


