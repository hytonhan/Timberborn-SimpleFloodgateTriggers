using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Timberborn.AssetSystem;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.EntitySystem;
using Timberborn.SelectionSystem;
using Timberborn.SingletonSystem;
using Timberborn.WaterBuildings;
using TimberbornAPI.AssetLoaderSystem.AssetSystem;
using TimberbornAPI.Common;
using TimberbornAPI.UIBuilderSystem;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.Length.Unit;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class AttachToStreamGaugeFragment
    {
        private readonly UIBuilder _builder;
        private readonly AttachToStreamGaugeButton _attachToStreamGaugeButton;
        private FloodgateTriggerMonoBehaviour _floodgateTriggerMonoBehaviour;

        private VisualElement _linksScrollView;
        private Label _noLinks;
        private Sprite _streamGaugeSprite;

        private readonly SelectionManager _selectionManager;
        StreamGaugeFloodgateLinkViewFactory _streamGaugeFloodgateLinkViewFactory;

        public AttachToStreamGaugeFragment(
            AttachToStreamGaugeButton attachToStreamGaugeButton,
            UIBuilder builder,
            SelectionManager selectionManager,
            StreamGaugeFloodgateLinkViewFactory streamGaugeFloodgateLinkViewFactory)
        {
            _attachToStreamGaugeButton = attachToStreamGaugeButton;
            _builder = builder;
            _selectionManager = selectionManager;
            _streamGaugeFloodgateLinkViewFactory = streamGaugeFloodgateLinkViewFactory;
        }


        public VisualElement InitiliazeFragment(VisualElement parent)
        {
            _streamGaugeSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                  .Where(x => x.name.StartsWith("StreamGauge"))
                                                  .SingleOrDefault();

            var root = _builder.CreateComponentBuilder()
                               .CreateVisualElement()
                               .SetName("LinksScrollView")
                               .SetWidth(new Length(290, Pixel))
                               .SetJustifyContent(Justify.Center)
                               .SetMargin(new Margin(0, 0, new Length(7, Pixel), 0))
                               .BuildAndInitialize();

            _attachToStreamGaugeButton.Initialize(parent, () => _floodgateTriggerMonoBehaviour, delegate
            {
                RemoveAllStreamGaugeViews();
                AddAllStreamGaugeViews();
            });

            _noLinks = _builder.CreateComponentBuilder()
                               .CreateLabel()
                               .AddPreset(factory => factory.Labels()
                                                            .GameTextBig(name: "NoLinksLabel",
                                                                         locKey: "Floodgates.Triggers.NoLinks",
                                                                         builder: builder => 
                                                                            builder.SetStyle(style => 
                                                                                style.alignSelf = Align.Center)))
                               .BuildAndInitialize();

            _linksScrollView = root.Q<VisualElement>("LinksScrollView");

            return root;
        }

        public void ClearFragment()
        {
            _floodgateTriggerMonoBehaviour = null;
            RemoveAllStreamGaugeViews();
        }

        public void ShowFragment(FloodgateTriggerMonoBehaviour floodgateTriggerMonoBehaviour)
        {
            _floodgateTriggerMonoBehaviour = floodgateTriggerMonoBehaviour;
            AddAllStreamGaugeViews();
        }

        public void UpdateFragment()
        {
            if ((bool)_floodgateTriggerMonoBehaviour)
            {
                //_floodgateTriggerMonoBehaviour.
                //TODO
                //Update streamgauges
            }
        }

        public void AddAllStreamGaugeViews()
        {
            ReadOnlyCollection<StreamGaugeFloodgateLink> links = _floodgateTriggerMonoBehaviour.FloodgateLinks;
            FloodGatesPlugin.Logger.LogInfo($"link count: {links.Count}");
            foreach (var link in links)
            {
                var streamGauge = link.StreamGauge.gameObject;
                var view = _streamGaugeFloodgateLinkViewFactory.CreateViewForFloodgate();

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                img.sprite = _streamGaugeSprite;
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                targetButton.clicked += delegate
                {
                    _selectionManager.FocusOn(streamGauge);
                };

                view.Q<Button>("DetachLinkButton").clicked += delegate
                {
                    link.Floodgate.DetachLink(link);
                    ResetLinks();
                };

                _linksScrollView.Add(view);
            }

            _attachToStreamGaugeButton.UpdateRemainingSlots(links.Count, _floodgateTriggerMonoBehaviour.MaxStreamGaugeLinks);
            if(links.IsEmpty())
            {
                _linksScrollView.Add(_noLinks);
            }
        }

        public void ResetLinks()
        {
            RemoveAllStreamGaugeViews();
            AddAllStreamGaugeViews();
            UpdateFragment();
        }

        public void RemoveAllStreamGaugeViews()
        {
            _linksScrollView.Clear();
        }

    }
}
