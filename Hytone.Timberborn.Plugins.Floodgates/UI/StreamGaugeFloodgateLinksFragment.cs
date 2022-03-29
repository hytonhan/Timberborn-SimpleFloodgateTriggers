using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using System.Collections.ObjectModel;
using System.Linq;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.EntityPanelSystem;
using Timberborn.SelectionSystem;
using TimberbornAPI.UIBuilderSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hytone.Timberborn.Plugins.Floodgates.UI
{
    public class StreamGaugeFloodgateLinksFragment : IEntityPanelFragment
    {
        private readonly UIBuilder _builder;

        private VisualElement _root;
        private ScrollView _floodgatesLinks;
        private Label _noLinks;

        private StreamGaugeMonoBehaviour _streamGaugeMonoBehaviour;

        private VisualElement _linksView;
        private Sprite _floodgateSprite;

        StreamGaugeFloodgateLinkViewFactory _streamGaugeFloodgateLinkViewFactory;
        private readonly SelectionManager _selectionManager;

        public StreamGaugeFloodgateLinksFragment(
            UIBuilder builder,
            StreamGaugeFloodgateLinkViewFactory streamGaugeFloodgateLinkViewFactory,
            SelectionManager selectionManager)
        {
            _builder = builder;
            _streamGaugeFloodgateLinkViewFactory = streamGaugeFloodgateLinkViewFactory;
            _selectionManager = selectionManager;
        }

        public VisualElement InitializeFragment()
        {
            _floodgateSprite = (Sprite)Resources.LoadAll("Buildings", typeof(Sprite))
                                                .Where(x => x.name.StartsWith("Floodgate"))
                                                .FirstOrDefault();
            var rootbuilder =
                _builder.CreateFragmentBuilder()
                        .AddComponent(
                            _builder.CreateComponentBuilder()
                                    .CreateVisualElement()
                                    .SetJustifyContent(Justify.Center)
                                    .SetName("FloodgatesContainer")
                                    .AddComponent(
                                        _builder.CreateComponentBuilder()
                                                .CreateLabel()
                                                .AddPreset(
                                                    factory => factory.Labels()
                                                                      .GameTextBig(locKey: "Floodgates.Triggers.LinkedFloodgates",
                                                                                   builder: builder => builder.SetStyle(style => style.alignSelf = Align.Center)))
                                                .BuildAndInitialize())
                                    .AddComponent(
                                        _builder.CreateComponentBuilder()
                                                .CreateScrollView()
                                                .AddPreset(
                                                    factory => factory.ScrollViews()
                                                                      .MainScrollView(name: "FloodgateLinks"))
                                                .BuildAndInitialize())
                                    .BuildAndInitialize());

            _root = rootbuilder.BuildAndInitialize();
            _floodgatesLinks = _root.Q<ScrollView>("FloodgateLinks");

            _noLinks = _builder.CreateComponentBuilder()
                               .CreateLabel()
                               .AddPreset(factory => factory.Labels()
                                                            .GameTextBig(name: "NoLinksLabel",
                                                                         locKey: "Floodgates.Triggers.NoFloodgateLinks",
                                                                         builder: builder =>
                                                                            builder.SetStyle(style =>
                                                                                style.alignSelf = Align.Center)))
                               .BuildAndInitialize();

            _root.ToggleDisplayStyle(visible: false);
            return _root;
        }

        public void ShowFragment(GameObject entity)
        {
            _streamGaugeMonoBehaviour = entity.GetComponent<StreamGaugeMonoBehaviour>();
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

            _floodgatesLinks.Clear();

            foreach (var link in links)
            {
                var floodgate = link.Floodgate.gameObject;
                var view = _streamGaugeFloodgateLinkViewFactory.CreateViewForStreamGauge();

                var imageContainer = view.Q<VisualElement>("ImageContainer");
                var img = new Image();
                img.sprite = _floodgateSprite;
                imageContainer.Add(img);

                var targetButton = view.Q<Button>("Target");
                targetButton.clicked += delegate
                {
                    _selectionManager.FocusOn(floodgate);
                };

                view.Q<Button>("DetachLinkButton").clicked += delegate
                {
                    link.Floodgate.DetachLink(link);
                    UpdateLinks();
                };

                _floodgatesLinks.Add(view);
            }
            if (links.IsEmpty())
            {
                _floodgatesLinks.Add(_noLinks);
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
