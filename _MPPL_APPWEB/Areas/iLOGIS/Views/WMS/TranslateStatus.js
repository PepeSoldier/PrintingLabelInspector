function TranslateStatus(status) {

    switch (status) {
        case 0: return "Zakończono pomyślnie";
        case 100: return "100: Lokacja nie została znaleziona";
        case 120: return "120: Lokacja jest Pełna";
        case 125: return "125: Na lokacji nie ma już wystarczjąco miejsca";
        case 140: return "140: Dla niektórych artykułów nie znaleziono lokacji";
        case 150: return "150: Lokacja nie jest taka sama";
        case 200: return "200: Opakowanie nie znalezione";
        case 201: return "201: jednostka magazynowa stworzona";
        case 202: return "202: jednostka magazynowa usunięta";
        case 203: return "203: jednostka magazynowa zaktualizowana";
        case 205: return "205: Jednostka magazynowa już istnieje";
        case 210: return "210: Opakowanie zawiera mniejszą ilość niż wymagana";
        case 212: return "212: Wprowadz ilość większą od 0";
        case 213: return "213: Problem podczas konwersji jednostek";
        case 214: return "214: Zła jednostka miary dla artykułu";
        case 220: return "220: Usuwanie jednostki magazynowej nie powiodło się";
        case 230: return "230: Żądana ilość zarezerwowana lub niedostępna";
        case 300: return "300: Artykuł nie został znaleziony w Magazynie";
        case 310: return "310: Artykuły nie są takie same";
        case 400: return "400: Nie znaleziono informacji o sposobie pakowania";
        case 500: return "500: Nie znaleziono dostawcy";
        case 600: return "600: Dostawa nie została znaleziona";
        case 610: return "610: Dostawca nieodpowiedni dla dostawy";
        case 700: return "700: Stanowisko nie zostało znalenione";
        case 730: return "730: Magazyn nie został znaleziony";
        case 731: return "731: Magazyn źródłowy nie został znaleziony";
        case 732: return "732: Magazyn docelowy nie został znaleziony";
        case 900: return "900: Problem z wydrukiem";
        case 901: return "901: Wykonano bez wydruku";
        case 902: return "902: Wydrukowano Etykietę";
        case 903: return "903: Nie można wydrukować etykiety";
        default: return status + ": nieznany błąd";
    }
}
