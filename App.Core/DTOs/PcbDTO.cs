using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.DTOs;

// Das Data Transfer Object (DTO) "PcbDTO" repräsentiert eine abgespeckte Version der Pcb-Entität für den Datenfluss.
// Es handelt sich um eine Keyless-Entität, da sie nicht über einen primären Schlüssel verfügt.
[Keyless]
public class PcbDTO
{
    // ID des PCB
    public int PcbId { get; set; }

    // Name des Speicherorts
    public string StorageName { get; set; }
    // Verweildauer
    public int DwellTime { get; set; }

    // Status der Verweildauer
    public int DwellTimeStatus { get; set; }

    //  Verweildauergrenze für Rot
    public string DwellTimeRed { get; set; }
    // Verweildauergrenze für Gelb
    public string DwellTimeYellow { get; set; }


    private string _dwellTimeColor;

    // Farbe der Verweildauer (nicht in der Datenbank gespeichert)
    [NotMapped]
    public string DwellTimeColor
    {
        get => getColor();
        set
        {
            _dwellTimeColor = value;
        }
    }

    // Zeitpunkt des Fehlers
    public DateTime FailedAt { get; set; }

    // Gibt an, ob das PCB abgeschlossen ist
    public bool IsFinalized { get; set; }

    // Seriennummer des PCBs
    public string SerialNumber { get; set; }

    // Teilenummer des PCBs
    public string PcbPartNumber { get; set; }

    //public string Error { get; set; }

    // Anzahl der Transfers
    public int TransferCount { get; set; }

    // Hauptfehlercode
    public string MainErrorCode { get; set; }

    // Unterfehlercode
    public string SubErrorCode { get; set; }

    public PcbDTO() { }

    // Methode zur Bestimmung der Farbe der Verweildauer basierend auf den Grenzwerten für Rot und Gelb
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


