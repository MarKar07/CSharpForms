# Tekstiseikkailu Windows Forms -sovellus

## Sovelluksen toiminta ja käyttötarkoitus

Tämä sovellus on graafinen tekstiseikkailu, jossa pelaaja voi tutkia linnaa, löytää esineitä ja kohdata erilaisia valintoja. Sovellus on toteutettu C#-kielellä ja Windows Forms -teknologialla.

Pelissä pelaaja:
- Navigoi linnan eri huoneissa
- Kerää esineitä (avain, kirja, kultakolikot)
- Ratkaisee ongelmia (lukittu ovi vaatii avaimen)
- Pyrkii pakenemaan linnasta mahdollisimman rikkaana

## Vuokaavio sovelluksesta
![vuo](https://github.com/user-attachments/assets/0799a8d6-e42d-48ee-b0f3-3713c8486c28)

## Kuvakaappaukset sovelluksesta

![Näyttökuva 2025-06-04 082623](https://github.com/user-attachments/assets/b8ceb503-0c96-45a1-a7d8-bd16e3460398)

![Näyttökuva 2025-06-04 082714](https://github.com/user-attachments/assets/e7fb8b22-8da6-439b-8a2b-a7a8b32f072a)

![Näyttökuva 2025-06-04 082754](https://github.com/user-attachments/assets/50c23bc9-75b5-4c13-9fcd-f4721f631dbd)

## Jatkokehitysideat
- Musiikin ja äänien lisääminen
- Vihollisten ja taistelun lisääminen
- Lisää huoneita
- Ulkoasun parantaminen
- Hahmonkehityksen luominen

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

