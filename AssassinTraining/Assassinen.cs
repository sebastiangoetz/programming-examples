class Spiel
{
    Dictionary<int,Assassine> assassins;
    Assassine spieler;

    public Spiel2()
    {
        System.out.println("Hallo Welt");
        spieler = new Assassine(1);
        spieler.setSpiel(this);
        assassins = new Dictionary<int, Assassine>();
        assassins.Add(1, spieler);
        for(int i = 2; i <= 5; i++)
        {
            Assassine x = new Assassine(i);
            x.setSpiel(this);
            assassins.Add(i, x);
        }
    }

    public Assassine getRandomAssassine(Assassine denAberNicht)
    {
        Random random = new Random();
        Assassine? partner;
        do
        {
            partner = assassins.GetValueOrDefault(random.Next(assassins.Count)-1);
        } while (partner == null || partner.Nummer == denAberNicht.Nummer);

        return partner;
    }

    public void GameLoop()
    {
        while(true)
        {
            Console.WriteLine("Deine aktuellen Werte: Kampf = " + spieler.FightLevel + " Verstecken = " + spieler.HideLevel);
            Console.WriteLine("Deine aktuelle Strategie: " + spieler.getStrategie().GetType().Name);
            Console.Write("> ");
            string? cmd = Console.ReadLine();
            if (cmd != null)
            {
                if (cmd == "exit") return;
                if (cmd == "kampf")
                {
                    spieler.setStrategie(new KampfTraining());
                }
                if (cmd == "hide")
                {
                    spieler.setStrategie(new VersteckTraining());
                }
            }
            //lass alle Assassinen ihre Routine verfolgen
            foreach (Assassine a in assassins.Values)
            {
                a.Routine();
                if (a.kannGutGenugVerstecken())
                    a.setStrategie(new KampfTraining());
                if (a.kannGutGenugKämpfen())
                    a.setStrategie(new VersteckTraining());
            }

            //gebe den aktuellen Stand auf die Konsole aus
            foreach (Assassine a in assassins.Values)
            {
                Console.Write(a.Nummer + " (" + a.HideLevel + "|" + a.FightLevel + ") ");
            }
            Console.WriteLine();
        }
    }

    static void Main()
    {
        Spiel spiel = new Spiel();
        spiel.GameLoop();
    }

}

class Assassine
{
    private Spiel? spiel;
    public int Nummer { get; set; }

    public int FightLevel { get; set; }
    public int HideLevel { get; set; }

    private Training trainingStrategie;

    public void setSpiel(Spiel spiel)
    {
        this.spiel = spiel;
    }

    public Assassine(int nummer)
    {
        this.Nummer = nummer;
        trainingStrategie = new VersteckTraining();
        FightLevel = 1;
        HideLevel = 1;
    }

    public void Routine()
    {
        if (spiel != null)
        {
            Assassine partner = spiel.getRandomAssassine(this);
            trainingStrategie.Trainieren(this, partner);
        }
    }

    public void setStrategie(Training trainingStrategie)
    {
        this.trainingStrategie = trainingStrategie;
    }

    public Training getStrategie()
    {
        return trainingStrategie;
    }

    public bool kannGutGenugKämpfen()
    {
        return FightLevel >= 10;
    }

    public bool kannGutGenugVerstecken()
    {
        return HideLevel >= 10;
    }
}

abstract class Training
{
    public abstract void Trainieren(Assassine a, Assassine b);
}

class VersteckTraining : Training
{
    public override void Trainieren(Assassine a, Assassine b)
    {
        Console.WriteLine(a.Nummer+" und "+b.Nummer+" trainieren verstecken.");
        a.HideLevel++;
        b.HideLevel++;
    }
}

class KampfTraining : Training
{
    public override void Trainieren(Assassine a, Assassine b)
    {
        Console.WriteLine(a.Nummer + " und " + b.Nummer + " trainieren den Kampf.");
        a.FightLevel++;
        b.FightLevel++;
    }
}
