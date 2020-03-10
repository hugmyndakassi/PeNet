﻿using PeNet.FileParser;
using PeNet.Structures;

namespace PeNet.Parser
{
    internal class ImageLoadConfigDirectoryParser : SafeParser<ImageLoadConfigDirectory>
    {
        private readonly bool _is64Bit;

        internal ImageLoadConfigDirectoryParser(IRawFile peFile, uint offset, bool is64Bit) 
            : base(peFile, offset)
        {
            _is64Bit = is64Bit;
        }

        protected override ImageLoadConfigDirectory ParseTarget()
        {
            return new ImageLoadConfigDirectory(PeFile, Offset, _is64Bit);
        }
    }
}