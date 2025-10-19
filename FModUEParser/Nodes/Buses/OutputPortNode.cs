using System;
using System.Collections.Generic;
using System.Text;

namespace FModUEParser.Nodes.Buses;

public class OutputPortNode(BinaryReader Ar) : BaseBusNode(Ar, true);
