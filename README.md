# Tekstiseikkailupeli englanniksi

## Sovelluksen toiminta ja käyttötarkoitus
Tämä tekstiseikkailu on konsolipohjainen roolipeli, jossa pelaaja tutkii vanhaa linnaa. Pelaaja voi liikkua huoneesta toiseen, löytää esineitä, kerätä kultaa ja lopulta paeta linnasta. Pelin tarkoitus on viihdyttää ja tarjota interaktiivinen tarinakokemus.

## Sovelluksen vuokaavio
[Lisää kuva vuokaaviosta tähän]

## Kuvakaappaukset sovelluksesta
![save_game2](https://github.com/user-attachments/assets/9cb3893c-c935-44e4-9baa-eb7c26cc2f13)  <br>
![save_game1](https://github.com/user-attachments/assets/734b310f-3054-4fd2-8ee2-d4dab61c9378)  <br>
![help](https://github.com/user-attachments/assets/290dfdd0-7331-485a-8f55-32284e45f3cf)  <br>
![stats](https://github.com/user-attachments/assets/1470915f-efda-460f-b345-d398b0639112)  <br>
![end](https://github.com/user-attachments/assets/be4825e1-8397-472c-8816-a0df5cb8ddc5)  <br>

## Koodin pääkohdat

Pelin voin tallentaa ja sen voin ladata myöhemmin. "help"-komennolla peli antaa apua.

### Player-luokka
Hallinnoi pelaajan tietoja kuten terveyttä, kultaa ja tavaraluetteloa.

```csharp
public class Player
{
    public int Health { get; set; }
    public int Gold { get; set; }
    public List<Item> Inventory { get; set; }
    public string CurrentLocation { get; set; }
    
    // Metodeita pelaajan toimintoihin...
}

public class Game
{
    // Pelin käynnistys ja hallinta...
    
    // Tallennusjärjestelmä...
    
    // Huoneet ja siirtymät...
}
