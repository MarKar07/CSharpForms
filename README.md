# Tekstiseikkailu Windows Forms -sovellus

## Sovelluksen toiminta ja käyttötarkoitus

Tämä sovellus on graafinen tekstiseikkailu, jossa pelaaja voi tutkia linnaa, löytää esineitä ja kohdata erilaisia valintoja. Sovellus on toteutettu C#-kielellä ja Windows Forms -teknologialla.

Pelissä pelaaja:
- Navigoi linnan eri huoneissa
- Kerää esineitä (avain, kirja, kultakolikot)
- Ratkaisee ongelmia (lukittu ovi vaatii avaimen)
- Pyrkii pakenemaan linnasta mahdollisimman rikkaana

## Vuokaavio sovelluksesta

## Kuvakaappaukset sovelluksesta

## Pääkohdat koodista

### Pelilogiikka

Pelin keskeinen logiikka on toteutettu Form1.cs-tiedostossa, jossa DisplayLocation-metodi vastaa pelimaailman näyttämisestä:

```csharp
private void DisplayLocation(string location)
{
    player.CurrentLocation = location;
    
    // Päivitä sijainnin kuva
    UpdateLocationImage(location);
    
    storytext.Clear();
    
    switch (location)
    {
        case "Entrance":
            // Sisäänkäynnin logiikka
            break;
        case "MainHall":
            // Pääaulan logiikka
            break;
        // jne.
    }
}

Player-luokka hallinnoi pelaajan tietoja kuten inventaariota ja sijaintia:

public class Player
{
    public int Health { get; set; }
    public int Gold { get; set; }
    public List<Item> Inventory { get; set; }
    public string CurrentLocation { get; set; }
    // jne.
}
