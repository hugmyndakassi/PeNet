﻿using PeNet.FileParser;

namespace PeNet.Structures
{
    /// <summary>
    ///     The IMAGE_IMPORT_BY_NAME structure is used to
    ///     describes imports of functions or symbols by their name.
    ///     The AddressOfData in the IMAGE_THUNK_DATA from the
    ///     IMAGE_IMPORT_DESCRIPTOR points to it.
    /// </summary>
    public class ImageImportByName : AbstractStructure
    {
        /// <summary>
        ///     Create new IMAGE_IMPORT_BY_NAME object.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset of the IMAGE_IMPORT_BY_NAME.</param>
        public ImageImportByName(IRawFile peFile, uint offset)
            : base(peFile, offset)
        {
        }

        /// <summary>
        ///     Hint.
        /// </summary>
        public ushort Hint
        {
            get => PeFile.ReadUShort(Offset);
            set => PeFile.WriteUShort(Offset, value);
        }

        /// <summary>
        ///     Name of the function to import as a C-string (null terminated).
        /// </summary>
        public string Name => PeFile.ReadAsciiString(Offset + 0x2);
    }
}