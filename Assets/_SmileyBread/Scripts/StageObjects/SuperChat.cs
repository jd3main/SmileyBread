using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

namespace _SmileyBread
{

    public class SuperChat : StageObject
    {
        public enum Level
        {
            Blue,
            Cyan,
            Green,
            Yellow,
            Orange,
            Purple,
            Red,
        }

        public static Dictionary<Level,Color> TitleBackgroundColors = new Dictionary<Level, Color>() {
            {Level.Blue, new Color(0.082f, 0.396f, 0.753f) },
            {Level.Cyan, new Color(0, 0.722f, 0.831f)},
            {Level.Green, new Color(0, 0.749f, 0.647f)},
            {Level.Yellow, new Color(1, 0.702f, 0)},
            {Level.Orange, new Color(0.902f, 0.318f, 0)},
            {Level.Purple, new Color(0.761f, 0.094f, 0.357f)},
            {Level.Red, new Color(0.816f, 0, 0)},
        };


        public float price = 10;
        public Level level = Level.Blue;
        public Color titleBackgroundColor = TitleBackgroundColors[Level.Blue];

        public string userName = "<userName>";
        public string content = "<content>";

        public Transform titleBackground;
        public Transform contentBackground;
        public TMP_Text userNameTextMesh;
        public TMP_Text priceTextMesh;
        public TMP_Text contentTextMesh;

        private void OnValidate()
        {
            titleBackgroundColor = TitleBackgroundColors[level];
        }

        private void Start()
        {
            userNameTextMesh.text = userName;
            priceTextMesh.text = $"${price.ToString("0,0.00", CultureInfo.InvariantCulture)}";
            contentTextMesh.text = content;
        }

        override public void Init(Stage stage)
        {
            float time = stage.time;
        }
    }
}