using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace BeyondBiosculpting
{
    public class Window_SelectItem<T> : Window
    {
        private Vector2 scrollPosition;
        public override Vector2 InitialSize => new Vector2(620f, 500f);

        public List<T> allItems;
        public Action<T> actionOnSelect;
        public Func<T, int> ordering;
        public Func<T, string> labelGetter;
        public Window_SelectItem(List<T> items, Action<T> actionOnSelect, Func<T, int> ordering = null, Func<T, string> labelGetter = null)
        {
            doCloseButton = true;
            doCloseX = true;
            closeOnClickedOutside = true;
            absorbInputAroundWindow = false;
            this.allItems = items;
            this.actionOnSelect = actionOnSelect;
            this.ordering = ordering;
            this.labelGetter = labelGetter;
        }
        string searchKey;
        public string GetLabel(T item) => labelGetter != null ? labelGetter(item) : item is Def def ? def.label : "";
        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;

            Text.Anchor = TextAnchor.MiddleLeft;
            var searchLabel = new Rect(inRect.x, inRect.y, 60, 24);
            Widgets.Label(searchLabel, "BB.Search".Translate());
            var searchRect = new Rect(searchLabel.xMax + 5, searchLabel.y, 200, 24f);
            searchKey = Widgets.TextField(searchRect, searchKey);
            Text.Anchor = TextAnchor.UpperLeft;

            Rect outRect = new Rect(inRect);
            outRect.y = searchRect.yMax + 5;
            outRect.yMax -= 70f;
            outRect.width -= 16f;

            var items = searchKey.NullOrEmpty() ? allItems : allItems.Where(x => GetLabel(x).ToLower().Contains(searchKey.ToLower())).ToList();

            Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, (float)items.Count() * 35f);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            try
            {
                float num = 0f;
                if (ordering != null)
                {
                    items = items.OrderBy(x => ordering(x)).ThenBy(x => GetLabel(x)).ToList();
                }
                foreach (T item in items)
                {
                    Rect iconRect = new Rect(0f, num, 0, 32);
                    if (item is Def def)
                    {
                        iconRect.width = 24;
                        Widgets.InfoCardButton(iconRect, def);
                    }
                    if (item is ThingDef thingDef2)
                    {
                        iconRect.x += 24;
                        Widgets.ThingIcon(iconRect, thingDef2);
                    }
                    Rect rect = new Rect(iconRect.xMax + 5, num, viewRect.width * 0.7f, 32f);
                    Text.Anchor = TextAnchor.MiddleLeft;
                    Widgets.Label(rect, GetLabel(item));
                    Text.Anchor = TextAnchor.UpperLeft;
                    rect.x = rect.xMax + 10;
                    rect.width = 100;
                    if (Widgets.ButtonText(rect, "BB.Select".Translate()))
                    {
                        actionOnSelect(item);
                        SoundDefOf.Click.PlayOneShotOnCamera();
                        this.Close();
                    }
                    num += 35f;
                }
            }
            finally
            {
                Widgets.EndScrollView();
            }
        }
    }
}
