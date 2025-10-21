using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes.Buses;

public class MasterBusNode(BinaryReader Ar) : BaseBusNode(Ar, FModReader.Version >= 0x49);
