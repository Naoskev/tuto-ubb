using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public static class SaveManager{

    public static string SaveWorld(this World world){
        using(TextWriter writer = new StringWriter()){

           XmlWriter xmlWriter = XmlWriter.Create(writer);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("World");
            xmlWriter.WriteAttributeString("Height", world.Height.ToString());
            xmlWriter.WriteAttributeString("Width", world.Width.ToString());
            
            xmlWriter.WriteStartElement("Tiles");
            world.ApplyToTiles((t) => t.SaveTile(xmlWriter));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Furnitures");
            world.ApplyToTiles((t) => t.SaveTile(xmlWriter));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("Characters");
            world.ApplyToTiles((t) => t.SaveTile(xmlWriter));
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            xmlWriter.Close();

            return writer.ToString(); 
        }
    }

    private static void SaveTile(this Tile tile, XmlWriter xmlWriter){        
        xmlWriter.WriteStartElement("Tile");
        xmlWriter.WriteAttributeString("X", tile.X.ToString());
        xmlWriter.WriteAttributeString("Y", tile.Y.ToString());
        xmlWriter.WriteAttributeString("Type", ((int)tile.Type).ToString());
        xmlWriter.WriteEndElement();
    }

    private static void SaveFurniture(Furniture furn, XmlWriter writer){

    }

    private static void SaveCharacter(Character character, XmlWriter writer){

    }


    public static World LoadWorld(string data){
        using(TextReader reader = new StringReader(data)){
            using(XmlReader xmlReader = XmlReader.Create(reader)){    
                xmlReader.ReadToFollowing("World");  
                int width = int.Parse(xmlReader.GetAttribute("Width")), height = int.Parse(xmlReader.GetAttribute("Height"));
                Logger.LogInfo(" x "+width+" y :"+height);
                World world = new World(width, height, true);

                while(xmlReader.Read()){
                    switch(xmlReader.Name){
                        case "Tile":
                            world.LoadTile(xmlReader);
                            break;
                        case "Furniture":
                            
                            break;
                        case "Character":

                            break;

                    }
                }

                return world;
            }

        }
    }

    private static void LoadTile(this World world, XmlReader xmlReader){
        int x = int.Parse(xmlReader.GetAttribute("X")), y = int.Parse(xmlReader.GetAttribute("Y"));

        Tile t = world.getTileAt(x, y);
        t.Type = (TileType) int.Parse(xmlReader.GetAttribute("Type"));
    }
    
    private static void LoadFurniture(this World world, XmlReader xmlReader){
        int x = int.Parse(xmlReader.GetAttribute("X")), y = int.Parse(xmlReader.GetAttribute("Y"));
        string furnId = xmlReader.GetAttribute("Id");
        Tile t = world.getTileAt(x, y);
        
    }

}