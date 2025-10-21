using FModBankParser.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FModBankParser.Nodes.Buses;

public class InputBusNode(BinaryReader Ar) : BaseBusNode(Ar, true);