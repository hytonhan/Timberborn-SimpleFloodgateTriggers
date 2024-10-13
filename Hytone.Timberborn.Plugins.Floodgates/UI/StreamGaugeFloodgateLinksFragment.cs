using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using System.Collections.ObjectModel;
using System.Linq;
using TimberApi.UIBuilderSystem;
using TimberApi.UIBuilderSystem.ElementBuilders;
using TimberApi.UIPresets.Builders;
using TimberApi.UIPresets.Labels;
using Timberborn.BaseComponentSystem;
using Timberborn.Buildings;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.EntitySystem;
using Timberborn.Localization;
using Timberborn.PrefabSystem;
using Timberborn.SelectionSystem;
using UnityEngine;
using UnityEngine.Categorization;
using UnityEngine.UIElements;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class StreamGaugeFloodgateLinksFragment : IEntityPanelFragment
    {
        private readonly UIBuilder _builder;
        private readonly ILoc _loc;

        private VisualElement _root;
        private ScrollView _links;
        private Label _noLinks;

        private StreamGaugeMonoBehaviour _streamGaugeMonoBehaviour;

        private VisualElement _linksView;
        private Sprite _floodgateSprite;
        private Sprite _waterpumpSprite;
        private Sprite _deepwaterpumpSprite;
        private Sprite _mechanicalWaterpumpSprite;
        private Sprite _deepmechanicalWaterpumpSprite;
        private Sprite _fluiddumpSprite;
        private Sprite _largeWaterPumpSprite;

        LinkViewFactory _streamGaugeFloodgateLinkViewFactory;
        private readonly EntitySelectionService _EntitySelectionService;

        public StreamGaugeFloodgateLinksFragment(
            ILoc loc,
            UIBuilder builder,
            LinkViewFactory streamGaugeFloodgateLinkViewFactory,
            EntitySelectionService EntitySelectionService)
        {
            _loc = loc;
            _builder = builder;
            _streamGaugeFloodgateLinkViewFactory = streamGaugeFloodgateLinkViewFactory;
            _EntitySelectionService = EntitySelectionService;
        }

        public VisualElement InitializeFragment()
        {
            _floodgateSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                .Where(x => x.name.StartsWith("Floodgate"))
                                                .FirstOrDefault();
            _waterpumpSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                .Where(x => x.name.StartsWith("WaterPump"))
                                                .FirstOrDefault();
            _deepwaterpumpSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                    .Where(x => x.name.StartsWith("DeepWaterPump"))
                                                    .FirstOrDefault();
            _mechanicalWaterpumpSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                          .Where(x => x.name.StartsWith("MechanicalWaterPump"))
                                                          .FirstOrDefault();
            _deepmechanicalWaterpumpSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                              .Where(x => x.name.StartsWith("DeepMechanicalWaterPump"))
                                                              .FirstOrDefault();
            _fluiddumpSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                .Where(x => x.name.StartsWith("FluidDump"))
                                                .FirstOrDefault();
            _largeWaterPumpSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                .Where(x => x.name.StartsWith("LargeWaterPump"))
                                                .FirstOrDefault();
                                                
            var rootbuilder = _builder.Create<VisualElementBuilder>()
                .AddComponent<FragmentBuilder>(builder => 
                    builder.AddComponent<VisualElementBuilder>(element => 
                        element.AddComponent<GameTextLabel>(label => label.Big().SetText(_loc.T("Floodgates.Triggers.LinkedFloodgates")))
                               .AddComponent<ScrollViewBuilder>("FloodgateLinks", scrollview => scrollview)
                    )
                );

            // var rootbuilder =
            //     _builder.CreateFragmentBuilder()
            //             .AddComponent(
            //                 _builder.CreateComponentBuilder()
            //                         .CreateVisualElement()
            //                         .SetJustifyContent(Justify.Center)
            //                         .SetName("FloodgatesContainer")
            //                         .AddComponent(
            //                             _builder.CreateComponentBuilder()
            //                                     .CreateLabel()
            //                                     .AddPreset(
            //                                         factory => factory.Labels()
            //                                                           .GameTextBig(locKey: "Floodgates.Triggers.LinkedFloodgates",
            //                                                                        builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
            //                                     .BuildAndInitialize())
            //                         .AddComponent(
            //                             _builder.CreateComponentBuilder()
            //                                     .CreateScrollView()
            //                                     .AddPreset(
            //                                         factory => factory.ScrollViews()
            //                                                           .MainScrollView(name: "FloodgateLinks"))
            //                                     .BuildAndInitialize())
            //                         .BuildAndInitialize());

            _root = rootbuilder.BuildAndInitialize();
            _links = _root.Q<ScrollView>("FloodgateLinks");

            _noLinks = _builder.Create<GameTextLabel>("NoLinksLabel")
                               .Big()
                               .SetText(_loc.T("Floodgates.Triggers.NoFloodgateLinks"))
                               .BuildAndInitialize();
            // _noLinks = _builder.CreateComponentBuilder()
            //                    .CreateLabel()
            //                    .AddPreset(factory => factory.Labels()
            //                                                 .GameTextBig(name: "NoLinksLabel",
            //                                                              locKey: "Floodgates.Triggers.NoFloodgateLinks",
            //                                                              builder: builder =>
            //                                                                 builder.SetStyle(style =>
            //                                                                     style.alignSelf = Align.Center)))
            //                    .BuildAndInitialize();

            _root.ToggleDisplayStyle(visible: false);
            return _root;
        }

        public void ShowFragment(BaseComponent entity)
        {
            _streamGaugeMonoBehaviour = entity.GetComponentFast<StreamGaugeMonoBehaviour>();
            if ((bool)_streamGaugeMonoBehaviour)
            {
                UpdateLinks();
            }
        }
        public void ClearFragment()
        {
            _streamGaugeMonoBehaviour = null;
            _root.ToggleDisplayStyle(visible: false);
        }

        public void UpdateFragment()
        {
            if ((bool)_streamGaugeMonoBehaviour)
            {
                _root.ToggleDisplayStyle(visible: true);
            }
        }

        /// <summary>
        /// Creates view for all existing floodgate links
        /// </summary>
        public void UpdateLinks()
        {
            ReadOnlyCollection<StreamGaugeFloodgateLink> links = _streamGaugeMonoBehaviour.FloodgateLinks;
            ReadOnlyCollection<WaterPumpStreamGaugeLink> waterpumplinks = _streamGaugeMonoBehaviour.WaterpumpLinks;
            ReadOnlyCollection<WaterSourceRegulatorStreamGaugeLink> waterSourceRegulatorLinks = _streamGaugeMonoBehaviour.WaterSourceRegulatorLinks;

            _links.Clear();

            foreach (var link in links)
            {
                var floodgate = link.Floodgate.GameObjectFast;
                var labeledPrefab = floodgate.GetComponent<Building>();
                var view = _streamGaugeFloodgateLinkViewFactory.CreateViewForStreamGauge(labeledPrefab.DisplayNameLocKey);

                var test =  (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                             .Where(x => x.name.StartsWith(labeledPrefab.name.Split('.')[0]))
                                             .FirstOrDefault();
                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                img.sprite = test;
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                targetButton.clicked += delegate
                {
                    _EntitySelectionService.SelectAndFocusOn(link.Floodgate);
                };

                view.Q<Button>("DetachLinkButton").clicked += delegate
                {
                    link.Floodgate.DetachLink(link);
                    UpdateLinks();
                };

                _links.Add(view);
            }
            foreach (var link in waterpumplinks)
            {
                //var r = new Regex(@"
                //(?<=[A-Z])(?=[A-Z][a-z]) |
                // (?<=[^A-Z])(?=[A-Z]) |
                // (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
                var waterpump = link.WaterPump.GameObjectFast;
                var labeledPrefab = waterpump.GetComponent<Building>();
                string waterpumpName = waterpump.name.Split('.').First();
                var view = _streamGaugeFloodgateLinkViewFactory.CreateViewForStreamGauge(labeledPrefab.DisplayNameLocKey);

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                
                var test =  (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                             .Where(x => x.name.StartsWith(labeledPrefab.name.Split('.')[0]))
                                             .FirstOrDefault();
                img.sprite = test;
                // switch (waterpumpName)
                // {
                //     case "WaterPump":
                //         img.sprite = _waterpumpSprite;
                //         break;
                //     case "DeepWaterPump":
                //         img.sprite = _deepwaterpumpSprite;
                //         break;
                //     case "MechanicalWaterPump":
                //         img.sprite = _mechanicalWaterpumpSprite;
                //         break;
                //     case "DeepMechanicalWaterpump":
                //         img.sprite = _deepmechanicalWaterpumpSprite;
                //         break;
                //     case "FluidDump":
                //         img.sprite = _fluiddumpSprite;
                //         break;
                //     case "LargeWaterPump":
                //         img.sprite = _largeWaterPumpSprite;
                //         break;
                //     default:
                //         break;

                // }
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                targetButton.clicked += delegate
                {
                    _EntitySelectionService.SelectAndFocusOn(link.WaterPump);
                };

                view.Q<Button>("DetachLinkButton").clicked += delegate
                {
                    link.WaterPump.DetachLink(link);
                    UpdateLinks();
                };

                _links.Add(view);
            }
            foreach (var link in waterSourceRegulatorLinks)
            {
                var waterSourceRegulator = link.WaterSourceRegulator.GameObjectFast;
                var labeledPrefab = waterSourceRegulator.GetComponent<Building>();
                string waterpumpName = waterSourceRegulator.name.Split('.').First();
                var view = _streamGaugeFloodgateLinkViewFactory.CreateViewForStreamGauge(labeledPrefab.DisplayNameLocKey);

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                
                var test =  (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                             .Where(x => x.name.StartsWith(labeledPrefab.name.Split('.')[0]))
                                             .FirstOrDefault();
                img.sprite = test;
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                targetButton.clicked += delegate
                {
                    _EntitySelectionService.SelectAndFocusOn(link.WaterSourceRegulator);
                };

                view.Q<Button>("DetachLinkButton").clicked += delegate
                {
                    link.WaterSourceRegulator.DetachLink(link);
                    UpdateLinks();
                };

                _links.Add(view);
            }
            if (links.IsEmpty() && waterpumplinks.IsEmpty() && waterSourceRegulatorLinks.IsEmpty())
            {
                _links.Add(_noLinks);
            }
            UpdateFragment();
        }

        /// <summary>
        /// Removes all existing floodagate link views
        /// </summary>
        public void RemoveAllStreamGaugeViews()
        {
            _linksView.Clear();
        }

    }
}
