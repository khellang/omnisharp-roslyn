// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Extensions.ContextQuery;
using Microsoft.CodeAnalysis.CSharp.Utilities;

namespace OmniSharp.Intellisense
{
    internal class ImplicitKeywordRecommender : AbstractSyntacticSingleKeywordRecommender
    {
        private static readonly ISet<SyntaxKind> s_validMemberModifiers = new HashSet<SyntaxKind>(SyntaxFacts.EqualityComparer)
            {
                SyntaxKind.StaticKeyword,
                SyntaxKind.PublicKeyword,
                SyntaxKind.ExternKeyword,
                SyntaxKind.UnsafeKeyword,
            };

        public ImplicitKeywordRecommender()
            : base(SyntaxKind.ImplicitKeyword)
        {
        }

        protected override bool IsValidContext(int position, CSharpSyntaxContext context, CancellationToken cancellationToken)
        {
            if (context.IsMemberDeclarationContext(validModifiers: s_validMemberModifiers, validTypeDeclarations: SyntaxKindSet.ClassStructTypeDeclarations, canBePartial: false, cancellationToken: cancellationToken))
            {
                // operators must be both public and static
                var modifiers = context.PrecedingModifiers;

                return
                    modifiers.Contains(SyntaxKind.PublicKeyword) &&
                    modifiers.Contains(SyntaxKind.StaticKeyword);
            }

            return false;
        }
    }
}
