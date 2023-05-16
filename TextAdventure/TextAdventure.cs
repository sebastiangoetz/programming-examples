class Game
{
    static void Main()
    {
        //Setup Level
        Raum startRaum = new Raum("Du wachst in deinem Schlafzimmer auf...");
        Raum wohnzimmer = new Raum("Du stehst im Wohnzimmer.");
        Raum kueche = new Raum("Du stehst in der Küche.");
        Raum bad = new Raum("Du stehst im Bad.");
        Raum flur = new Raum("Du bist im Flur. Von hier kommst du überall hin.");
        Raum hausflur = new Raum("Du stehst im Hausflur");

        Item linkerSchuh = new Item("Linker Schuh", 5);
        Item rechterSchuh = new Item("Rechter Schuh", 5);
        Item hose = new Item("Hose", 10);
        Item tshirt = new Item("T-Shirt", 10);

        hausflur.AddBenoetigtesItem(linkerSchuh);
        hausflur.AddBenoetigtesItem(rechterSchuh);
        hausflur.AddBenoetigtesItem(hose);
        hausflur.AddBenoetigtesItem(tshirt);

        bad.AddItem(hose);
        flur.AddItem(tshirt);
        kueche.AddItem(linkerSchuh);
        wohnzimmer.AddItem(rechterSchuh); 

        startRaum.SetVor(flur);
        flur.SetZurueck(startRaum);
        flur.SetLinks(wohnzimmer);
        wohnzimmer.SetRechts(flur);
        flur.SetRechts(kueche);
        kueche.SetLinks(flur);
        flur.SetVor(bad);
        bad.SetZurueck(flur);
        wohnzimmer.SetVor(hausflur);
        hausflur.SetZurueck(wohnzimmer);

        //Setup Player
        Spieler player1 = new Spieler("Player1",startRaum);

        //GameLoop
        while(true)
        {
            player1.GetAktuellePosition().Betreten();
            foreach(Item i in player1.GetAktuellePosition().GetItems()) {
                player1.AddItem(i);
                Console.WriteLine("Ich habe "+i.GetName()+" gefunden!");
            }
            player1.GetAktuellePosition().ClearItems();
            Console.Write("> ");
            string? befehl = Console.ReadLine();
            Raum? naechsterRaum = null;
            if (befehl == "links" || befehl == "l") naechsterRaum = player1.GetAktuellePosition().GetLinks();
            if (befehl == "rechts" || befehl == "r") naechsterRaum = player1.GetAktuellePosition().GetRechts();
            if (befehl == "vor" || befehl == "v") naechsterRaum = player1.GetAktuellePosition().GetVor();
            if (befehl == "zurueck" || befehl == "z") naechsterRaum = player1.GetAktuellePosition().GetZurueck();
            if (befehl == "inventar" || befehl == "i")
            {
                Console.Write("Ich habe in meinem Inventar: ");
                foreach (Item i in player1.GetInventar()) Console.Write(i.GetName() + " ");
                Console.WriteLine();
            }
            else
            if (befehl == "exit" || befehl == "quit") return;
            else
            if (befehl == "help" || befehl == "h")
                Console.WriteLine("Befehle: links (l), rechts (r), vor (v), zurueck (z), inventar (i), quit/exit");
            else
            if (naechsterRaum != null)
            {
                bool darfEintreten = true;
                foreach(Item i in naechsterRaum.GetBenoetigteItems())
                {
                    if(!player1.ContainsItem(i))
                    {
                        darfEintreten = false;
                        Console.WriteLine("Dir fehlt: " + i.GetName());
                    }
                }
                if(darfEintreten)
                    player1.SetAktuellePosition(naechsterRaum);
            }
            else
                Console.WriteLine("In diese Richtung geht es nicht weiter.");
        }
    }
}

class Spieler
{
    private string name;
    private Raum aktuellePosition;
    private List<Item> inventar;

    public Spieler(string name, Raum aktuellePosition)
    {
        this.name = name;
        this.aktuellePosition = aktuellePosition;
        inventar = new List<Item>();
    }

    public string GetName() { return name; }   
    public Raum GetAktuellePosition() { return aktuellePosition; }
    public void SetAktuellePosition(Raum r) { aktuellePosition = r; }

    public void AddItem(Item i) { inventar.Add(i); }
    public void RemoveItem(Item i) { inventar.Remove(i); }
    public bool ContainsItem(Item i) { return inventar.Contains(i); }
    public List<Item> GetInventar() { return inventar; }
}

class Raum
{
    private string text;
    private Raum rechts, links, vor, zurueck;
    private List<Item> items;
    private List<Item> benoetigteItems;

    public Raum(string text)
    {
        this.text = text;
        items = new List<Item>();
        benoetigteItems = new List<Item>();
    }

    public Raum(string text, Raum rechts, Raum links, Raum vor, Raum zurueck)
    {
        this.text = text;
        this.rechts = rechts;
        this.links = links;
        this.vor = vor;
        this.zurueck = zurueck;
    }

    public void SetRechts(Raum r) { rechts = r; }
    public void SetLinks(Raum r) { links = r; }
    public void SetVor(Raum r) { vor = r; }
    public void SetZurueck(Raum r) { zurueck = r; }

    public Raum GetRechts() { return rechts; }
    public Raum GetLinks() { return links; }    
    public Raum GetVor() { return vor; }
    public Raum GetZurueck() { return zurueck; }

    public void AddItem(Item i) { items.Add(i); }
    public void RemoveItem(Item i) { items.Remove(i); }
    public bool ContainsItem(Item i) { return items.Contains(i); }
    public List<Item> GetItems() { return items; }
    public void ClearItems() { items.Clear(); }

    public void AddBenoetigtesItem(Item i) { benoetigteItems.Add(i); }
    public List<Item> GetBenoetigteItems() { return benoetigteItems; }

    public void Betreten()
    {
        Console.WriteLine(text);
    }
}

class Item
{
    private string name;
    private int wert;

    public Item(string name, int wert)
    {
        this.name = name;
        this.wert = wert;
    }

    public string GetName() { return name; }
    public int GetWert() { return wert; }
    public void SetName(string name) { this.name = name; }
    public void SetWert(int wert) { this.wert = wert; }
}