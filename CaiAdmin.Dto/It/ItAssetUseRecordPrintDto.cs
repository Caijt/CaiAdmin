﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ItSys.Dto
{
    public class ItAssetUseRecordPrintDto :ItAssetUseRecordDto
    {
        public IEnumerable<ItAssetUseRecordItemDto> assets;
    }
}
