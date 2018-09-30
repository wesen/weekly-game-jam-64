using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class GhostInteraction {
    public Vector2 Position;
    public string Interaction;

    public GhostInteraction(Vector2 position, string interaction) {
        this.Interaction = interaction;
        this.Position = position;
    }
};

[System.Serializable]
public class PathObject {
    public ArrayList position;
    public string _id;
    public string name;
    public string room;
    public ArrayList movement;
    public ArrayList interaction;
    public string createDate;
    public Int32 __v;
};

public class GhostInformation {
    public Vector2 Position;
    public Vector2[] Movement;
    public GhostInteraction[] Interactions;
    public string Room;

    public static GhostInformation FromPathObject(PathObject pathObject) {
        GhostInformation information = new GhostInformation();
        information.Room = pathObject.room;
        information.Movement = pathObject.movement.Cast<JArray>().Select<JArray, Vector2>((JArray jarray) => {
            return new Vector2(jarray[0].Value<float>(), jarray[1].Value<float>());
        }).ToArray();
        information.Interactions = pathObject.interaction.Cast<JArray>().Select(jarray => {
            float x = jarray[0].Value<float>();
            float y = jarray[1].Value<float>();
            string interaction = jarray[2].Value<string>();
            return new GhostInteraction(new Vector2(x, y), interaction);
        }).ToArray();

        return information;
    }
};