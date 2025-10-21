using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes.Buses;

public class GroupBusNode(BinaryReader Ar) : BaseBusNode(Ar, true);