using Godot;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Linq;

public partial class main : Node
{
    // Called when the node enters the scene tree for the first time.
    PackedScene card = GD.Load<PackedScene>("res://card.tscn");
    // Card Class
    //All Required info relevent to load card (Name, ID, rarity, images)
    //public Vector2 ScreenSize;

    public void CreateNewCard(Texture2D imgLoc)
    {
        //Create new instance
        RigidBody2D newCard = (RigidBody2D)card.Instantiate();
        
        AddChild(newCard);

        Sprite2D sprite = newCard.GetChild(0) as Sprite2D;
        sprite.Texture = imgLoc;

        
        //newCard.Position = ScreenSize;

        //Rigid Body is frozen by default, on mouse click and hold grab and move, unreeze and allow physics to discard
        //Uninstance card when fully off screen
    }

    public class CardClass //Create Card Objects for each card that the "pack" contains
    {
        public string name { get; set; } //JSON "name"
        public string id { get; set; } //JSON "id"
        public string rarity { get; set; } //JSON "rarity"

        public CardClass(string _name, string _id, string _rarity)
        {
            name = _name;
            id = _id;
            rarity = _rarity;
        }
        //Card Objects will be created with for loop of the saved JSON data of the 10 cards that are set for the pack
        //Card Object will have an interactable node to be grabbed by the player hand to be tossed, deleted once off screen
    };

    //JSON -> LINQ
    //LINQ 10 random cards in pack to List (for now, we can weight rarity and types later when the concept is working)
    //JSON base1cards.json
    public void MakePack()
    {
        using (StreamReader reader = File.OpenText(@"./base1cards.json"))
        {
            JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));

            //Grab 10 Random cards //https://stackoverflow.com/questions/74631534/is-there-any-way-to-get-random-data-from-a-json-file-in-c-solved
            //Randomize the ID to grab, this can become wieghted later
            int length = (int)o["totalCount"];
            Random r = new Random();
            GD.Print("Length is:" + length);

            //Next steps -- 
                //mouse hold control to unfreeze, delete from memory when fully off screen
                //weighted packs built in order, one rare min
                

            for (int i = 0; i < 1; i++)
            {
                int rNumber = r.Next(length);

                string id = (string)o["data"][rNumber]["id"];
                GD.Print(id);
                string name = (string)o["data"][rNumber]["name"];
                GD.Print(name);
                string rarity = (string)o["data"][rNumber]["rarity"];
                GD.Print(rarity);

                Texture2D imgLoc = (Texture2D)GD.Load(string.Format("res://assets/base1/{0}.png", id));

                //Create Instance a scene of card instead 
                CreateNewCard(imgLoc);
            }
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PackedScene card = GD.Load<PackedScene>("res://card.tscn");
        base._Ready();
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsActionPressed("space"))
        {
            MakePack();
        }
    }
}
