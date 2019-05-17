using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Paper.Browser.Windows.Gui.Layouts;
using Paper.Browser.Windows.Lib;
using Paper.Media;

namespace Paper.Browser.Windows.Lib.Pages
{
  public interface IPage
  {
    Control Host { get; }

    Window Window { get; set; }

    Entity Entity { get; set; }
  }
}
