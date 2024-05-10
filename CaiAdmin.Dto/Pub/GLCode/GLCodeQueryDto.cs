using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Pub
{
    public class GLCodeQueryDto : QueryDto
    {
        public string Name { get; set; }

        public string IdString { get; set; }

        public GLCodeType[] Types { get; set; }

        public Guid[] Ids { get; set; }
    }
}
