using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModUEParser.Nodes.Buses;

public class ReturnBusNode(BinaryReader Ar) : BaseBusNode(Ar, true);