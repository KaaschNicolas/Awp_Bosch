using App.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Core.DTOs;

[Keyless]
public class PcbDTO
{
    public int PcbId { get; set; }

    public Pcb Pcb { get; set; }

    public string StorageName { get; set; }

    public int DwellTimeStatus { get; set; }

    public PcbDTO() { }
}


