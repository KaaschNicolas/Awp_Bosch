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


    private string _dwellTimeColor;

    [NotMapped]
    public string DwellTimeColor
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

    public string getColor()
    {
        string StatusColor;
        if (DwellTimeRed != "--" && DwellTimeYellow != "--")
        {

            if (DwellTime >= int.Parse(DwellTimeRed))
            {

                StatusColor = "red";

            }
            else if (DwellTime >= int.Parse(DwellTimeYellow))
            {
                StatusColor = "yellow";
            }
            else
            {
                StatusColor = "limegreen";

            }
            return StatusColor;
        }
        else
        {
            return "transparent";
        }
    }
}


