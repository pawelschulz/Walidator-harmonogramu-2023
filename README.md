# Walidator-harmonogramu-2023

Program "Walidator" na podstawie pliku źródłowego (w formacie: dzień miesiąca, liczba godzin) ma na celu odpowiedzieć na 4 pytania:
1) Czy suma przepracowanych godzin w miesiącu przekracza ilość [8h*liczba dni pracujących]?
2) Czy została zaplanowana praca na niedzielę?
3) Ile godzin nadgodzin?
4) Czy pomiędzy końcem jednego dnia, a początkiem następnego jest co najmniej 11h przerwy?

Składa się on z 2 klas (położonych w osobnych plikach: Program.cs i Day.cs):
- Program: zawiera metodę Main(), listę dni (ListOfDays), wczytuje dane z pliku i wywołuje potrzebne metody z klasy Day;
- Day: pozwala tworzyć obiekty Day (o atrybutach: nr dnia w miesiącu-month, nr dnia w tygodniu-week, liczba godzin-hours); zawiera metody umożliwiające: wczytanie danych z pliku, wypisanie odczytanych danych, określenie po nazwie nr dnia w tygodniu, wyciągnięcie ze stałych atrybutów obiektu Day informacji pomocniczych oraz metody służące do walidacji harmonogramu.
  
Oprócz warunków z treści zadania, na potrzeby realizacji programu przyjąłem, że:
- rok przyjmie wartość 1950-2050,
- dni pracujące są w zakresie pn-pt ( dot. 1) ),
- czas rozpoczęcia każdego dnia pracy to godzina 8:00 ( dot. 4) ),
- pracownik nie może pracować więcej niż 16h dziennie ( dot. 4) ),
- plik źródłowy nie zawiera błędów danych (wartości ujemnych, powtarzających się numerów dni, pustych linii).

Program poddany został testom jednostkowym:
- wprowadzono błędne dane dot. roku: abcd, 2057, 1000,
- wprowadzono błędne dane dot. miesiąca: abcd, -1, 15,
- wprowadzono nazwę nieistniejącego pliku,
- 
- w pliku harm3.in pozostawiono pustą linijkę
- w pliku harm4.in powtórzono nr dnia w miesiącu
- w pliku harm5.in zmieniono koljność numerów dni w miesiącu




