using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public abstract class EntrySymbol : CatalogueItemSymbol, IEntrySymbol
    {
        internal EntryBaseNode Declaration { get; }

        protected EntrySymbol(
            ISymbol containingSymbol,
            EntryBaseNode declaration,
            Binder binder,
            BindingDiagnosticContext diagnostics)
            : base(containingSymbol, declaration, diagnostics)
        {
            Declaration = declaration;
            // consider binding at later stage
            var publication = binder.BindPublicationSymbol(declaration);
            if (publication is not null)
            {
                PublicationReference = new EntryPublicationReferenceSymbol(this, publication);
            }
            Effects = LogicSymbol.CreateEffects(this, binder, diagnostics);
        }

        public bool IsHidden => Declaration.Hidden;

        protected virtual IEntrySymbol? BaseReferencedEntry => null;

        public IPublicationReferenceSymbol? PublicationReference { get; }

        public ImmutableArray<IEffectSymbol> Effects { get; }

        public bool IsReference => BaseReferencedEntry is not null;

        IEntrySymbol? IEntrySymbol.ReferencedEntry => BaseReferencedEntry;

        public static ImmutableArray<IResourceEntrySymbol> CreateResourceEntries(
            ICatalogueItemSymbol containingSymbol,
            ContainerEntryBaseNode node,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return Core().ToImmutableArray();

            IEnumerable<IResourceEntrySymbol> Core()
            {
                var costs = node switch
                {
                    SelectionEntryNode entry => entry.Costs.NodeList,
                    EntryLinkNode link => link.Costs.NodeList,
                    _ => default,
                };
                foreach (var item in costs)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
                foreach (var item in node.InfoGroups)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
                foreach (var item in node.InfoLinks)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
                foreach (var item in node.Profiles)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
                foreach (var item in node.Rules)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
            }
        }

        public static ImmutableArray<IResourceEntrySymbol> CreateResourceEntries(
            ICatalogueItemSymbol containingSymbol,
            InfoGroupNode node,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return Core().ToImmutableArray();

            IEnumerable<IResourceEntrySymbol> Core()
            {
                foreach (var item in node.InfoGroups)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
                foreach (var item in node.InfoLinks)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
                foreach (var item in node.Profiles)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
                foreach (var item in node.Rules)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
            }
        }

        public static ImmutableArray<ICategoryEntrySymbol> CreateCategoryEntries(
            ICatalogueItemSymbol containingSymbol,
            SelectionEntryBaseNode node,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return Core().ToImmutableArray();

            IEnumerable<ICategoryEntrySymbol> Core()
            {
                foreach (var item in node.CategoryLinks)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
            }
        }

        public static ImmutableArray<ISelectionEntryContainerSymbol> CreateSelectionEntryContainers(
            ICatalogueItemSymbol containingSymbol,
            SelectionEntryBaseNode node,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return Core().ToImmutableArray();

            IEnumerable<ISelectionEntryContainerSymbol> Core()
            {
                foreach (var item in node.EntryLinks)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
                foreach (var item in node.SelectionEntries)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
                foreach (var item in node.SelectionEntryGroups)
                {
                    yield return CreateEntry(containingSymbol, item, binder, diagnostics);
                }
            }
        }

        private static ISelectionEntryContainerSymbol CreateEntry(
            ICatalogueItemSymbol containingSymbol,
            EntryLinkNode item,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return new SelectionEntryLinkSymbol(containingSymbol, item, binder, diagnostics);
        }

        private static IRuleSymbol CreateEntry(
            ICatalogueItemSymbol containingSymbol,
            RuleNode item,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return new RuleSymbol(containingSymbol, item, binder, diagnostics);
        }

        private static IProfileSymbol CreateEntry(
            ICatalogueItemSymbol containingSymbol,
            ProfileNode item,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return new ProfileSymbol(containingSymbol, item, binder, diagnostics);
        }

        private static IResourceEntrySymbol CreateEntry(
            ICatalogueItemSymbol containingSymbol,
            InfoLinkNode item,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return new ResourceLinkSymbol(containingSymbol, item, binder, diagnostics);
        }

        private static IResourceGroupSymbol CreateEntry(
            ICatalogueItemSymbol containingSymbol,
            InfoGroupNode item,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return new ResourceGroupSymbol(containingSymbol, item, binder, diagnostics);
        }

        private static ICostSymbol CreateEntry(
            ICatalogueItemSymbol containingSymbol,
            CostNode item,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return new CostSymbol(containingSymbol, item, binder, diagnostics);
        }

        private static ICategoryEntrySymbol CreateEntry(
            ICatalogueItemSymbol containingSymbol,
            CategoryLinkNode item,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return new CategoryLinkSymbol(containingSymbol, item, binder, diagnostics);
        }

        public static ISelectionEntryContainerSymbol CreateEntry(
            ICatalogueItemSymbol containingSymbol,
            SelectionEntryNode node,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return new SelectionEntrySymbol(containingSymbol, node, binder, diagnostics);
        }

        public static ISelectionEntryContainerSymbol CreateEntry(
            ICatalogueItemSymbol containingSymbol,
            SelectionEntryGroupNode node,
            Binder binder,
            BindingDiagnosticContext diagnostics)
        {
            return new SelectionEntryGroupSymbol(containingSymbol, node, binder, diagnostics);
        }
    }
}
