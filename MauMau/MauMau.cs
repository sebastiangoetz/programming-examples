class TolleKarte
{
    public string Wert { get; set; }
    public string Farbe { get; set; }

    public bool Passt(Karte k)
    {
        if (k.Farbe == this.Farbe || k.Wert == this.Wert)
            return true;
        else
            return false;
    }

    public override string ToString()
    {
        return Farbe+" "+Wert;
    }

    public static Stack<Karte> ErzeugeKartensatz()
    {
        Stack<Karte> alleKarten = new Stack<Karte>();
        string[] farben = { "Herz", "Kreuz", "Pik", "Karo" };
        string[] werte = { "7", "8", "9", "10", "Bube", "Dame", "König", "Ass" };

        foreach (string farbe in farben)
        {
            foreach (string wert in werte)
            {
                Karte k = new Karte();
                k.Farbe = farbe;
                k.Wert = wert;
                alleKarten.Push(k);
            }
        }
        return alleKarten;
    }

    public override bool Equals(object? obj)
    {
        if(obj != null && obj is Karte)
        {
            Karte k = (Karte)obj;
            if (k.Farbe == this.Farbe && k.Wert == this.Wert) return true;
            else return false;
        }
        return false;
    }
}

class Spieler
{
    public string Name { get; set; }
    public List<Karte> kartenAufDerHand;

    public Spieler(string name)
    {
        Name = name;
        kartenAufDerHand = new List<Karte>();
    }

    public void NehmeKarte(Karte k)
    {
        kartenAufDerHand.Add(k);
    }

    public void LegeKarteAb(Karte k)
    {
        kartenAufDerHand.Remove(k);
    }

    public void ZeigeHand()
    {
        foreach (Karte karte in kartenAufDerHand)
        {
            Console.Write(karte + ", ");
        }
        Console.WriteLine();
    }
}

class Spiel
{
    public List<Spieler> Mitspieler;
    public Stack<Karte> Kartenstapel;
    public Stack<Karte> Ablagestapel;
    public Spieler aktuellerSpieler;
    private int aktuellerSpielerIndex;

    public Spiel()
    {
        Mitspieler = new List<Spieler>();
        Kartenstapel = Karte.ErzeugeKartensatz();
        Ablagestapel = new Stack<Karte>();
        Ablagestapel.Push(Kartenstapel.Pop());
    }

    public void AddSpieler(string name)
    {
        Spieler spieler = new Spieler(name);
        Mitspieler.Add(spieler);
    }

    public void GameLoop()
    {
        bool achtGelegt = false;
        int zuZiehendeKarten = 0;

        //Karten verteilen
        for(int i = 0; i < 5; i++)
        {
            foreach(Spieler spieler in Mitspieler)
            {
                spieler.NehmeKarte(Kartenstapel.Pop());
            }
        }

        aktuellerSpieler = Mitspieler.First();
        aktuellerSpielerIndex = 0;
        
        while(true)
        {
            Console.WriteLine("Oberste Karte: " + Ablagestapel.Peek());
            Console.Write(aktuellerSpieler.Name + ": ");
            aktuellerSpieler.ZeigeHand();
            Console.Write("> ");
            string? cmd = Console.ReadLine();
            if (cmd == null) continue;
            if (cmd == "exit") return;
            else if (cmd == "show")
            {
                aktuellerSpieler.ZeigeHand();
            } else
              if (cmd == "take")
              {
                if (achtGelegt)
                {
                    achtGelegt = false;
                    for(int i = 0; i < zuZiehendeKarten; i++)
                    {
                        aktuellerSpieler.NehmeKarte(Kartenstapel.Pop());
                    }
                } else
                {
                    aktuellerSpieler.NehmeKarte(Kartenstapel.Pop());
                }
                nächsterSpieler();
              }
            else
            if (cmd.StartsWith("Karo") ||
                cmd.StartsWith("Herz") ||
                cmd.StartsWith("Pik") ||
                cmd.StartsWith("Kreuz"))
            {
                if(cmd.Split(" ").Length == 2) //"Karo 8 ablegen".Split(" ") -> [0]: "Karo" [1]: "8" [2]: "ablegen"
                {
                    string farbe = cmd.Split(" ")[0];
                    string wert = cmd.Split(" ")[1];
                    //check ob Spieler die Karte besitzt
                    Karte k = new Karte();
                    k.Farbe = farbe;
                    k.Wert = wert;
                    if(!aktuellerSpieler.kartenAufDerHand.Contains(k))
                    {
                        Console.WriteLine("Sie haben diese Karte nicht!");
                        continue;
                    } else
                    {
                        //check ob Karte auf den Ablagestapel passt
                        if(Ablagestapel.Peek().Passt(k))
                        {
                            //check ob vorher eine 8 gelegt wurde
                            if (achtGelegt && k.Wert == "8" || !achtGelegt)
                            {
                                Ablagestapel.Push(k);
                                aktuellerSpieler.LegeKarteAb(k);
                            } else
                            {
                                Console.WriteLine("Sie müssen auch eine 8 legen oder eine Karte ziehen.");
                            }
                            if(k.Wert == "8")
                            {
                                achtGelegt = true;
                                zuZiehendeKarten++;
                            }
                            if(aktuellerSpieler.kartenAufDerHand.Count() == 0)
                            {
                                Console.WriteLine("Glückwunsch. Spieler " + aktuellerSpieler.Name + " hat gewonnen!");
                                return;
                            }
                            nächsterSpieler();
                        } else
                        {
                            Console.WriteLine("Die Karte passt nicht auf den Stapel.");
                        }
                    }
                } else
                {
                    Console.WriteLine("Bitte geben sie eine gütige Kartenbezeichnung ein.");
                }
            }
        }
    }

    private void nächsterSpieler()
    {
        aktuellerSpielerIndex++;
        if (aktuellerSpielerIndex == Mitspieler.Count) aktuellerSpielerIndex = 0;
        aktuellerSpieler = Mitspieler.ElementAt(aktuellerSpielerIndex);
    }

    static void Main()
    {
        Spiel spiel = new Spiel();
        spiel.AddSpieler("Anja");
        spiel.AddSpieler("Klaus");
        spiel.AddSpieler("Aldeida");

        spiel.GameLoop();
    }
}

