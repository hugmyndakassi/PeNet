﻿using System;
using PeNet.FileParser;
using PeNet.Utilities;

namespace PeNet.Structures
{
    /// <summary>
    ///     The resource directory entry represents one entry (e.g. icon)
    ///     in a resource directory.
    /// </summary>
    public class ImageResourceDirectoryEntry : AbstractStructure
    {
        /// <summary>
        ///     Create a new instance of the IMAGE_RESOURCE_DIRECTORY_ENTRY.
        /// </summary>
        /// <param name="peFile">A PE file.</param>
        /// <param name="offset">Raw offset to the entry.</param>
        /// <param name="resourceDirOffset">Raw offset to the resource directory.</param>
        public ImageResourceDirectoryEntry(IRawFile peFile, long offset, long resourceDirOffset)
            : base(peFile, offset)
        {
            // Resolve the Name
            try
            {
                if (IsIdEntry)
                {
                    ResolvedName = FlagResolver.ResolveResourceId(ID);
                }
                else if (IsNamedEntry)
                {
                    var nameAddress = resourceDirOffset + (Name & 0x7FFFFFFF);
                    var unicodeName = new ImageResourceDirStringU(PeFile, nameAddress);
                    ResolvedName = unicodeName.NameString;
                }
            }
            catch (Exception)
            {
                ResolvedName = null;
            }
        }

        /// <summary>
        ///     Get the Resource Directory which the Directory Entry points
        ///     to if the Directory Entry has DataIsDirectory set.
        /// </summary>
        public ImageResourceDirectory? ResourceDirectory { get; internal set; }

        /// <summary>
        ///     Get the Resource Data Entry if the entry is no directory.
        /// </summary>
        public ImageResourceDataEntry? ResourceDataEntry { get; internal set; }

        /// <summary>
        ///     Address of the name if its a named resource.
        /// </summary>
        public uint Name
        {
            get => PeFile.ReadUInt(Offset);
            set => PeFile.WriteUInt(Offset, value);
        }

        /// <summary>
        ///     The resolved name as a string if its a named resource.
        /// </summary>
        public string? ResolvedName { get; }

        /// <summary>
        ///     The ID if its a ID resource.
        ///     You can resolve the ID to a string with Utility.ResolveResourceId(id)
        /// </summary>
        public uint ID
        {
            get => Name & 0xFFFF;
            set => Name = value & 0xFFFF;
        }

        /// <summary>
        ///     Offset to the data.
        /// </summary>
        public uint OffsetToData
        {
            get => PeFile.ReadUInt(Offset + 0x4);
            set => PeFile.WriteUInt(Offset + 0x4, value);
        }

        /// <summary>
        ///     Offset to the next directory.
        /// </summary>
        public uint OffsetToDirectory => OffsetToData & 0x7FFFFFFF;

        /// <summary>
        ///     True if the entry data is a directory
        /// </summary>
        public bool DataIsDirectory
        {
            get
            {
                if ((OffsetToData & 0x80000000) == 0x80000000)
                    return true;
                return false;
            }
        }

        /// <summary>
        ///     True if the entry is a resource with a name.
        /// </summary>
        public bool IsNamedEntry
        {
            get
            {
                if ((Name & 0x80000000) == 0x80000000)
                    return true;
                return false;
            }
        }

        /// <summary>
        ///     True if the entry is a resource with an ID instead of a name.
        /// </summary>
        public bool IsIdEntry => !IsNamedEntry;
    }
}