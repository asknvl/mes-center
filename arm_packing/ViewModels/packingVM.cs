using Aspose.Words;
using Aspose.Words.Drawing;
using mes_center.Models.scanner;
using mes_center.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mes_center.arm_packing.ViewModels
{
    internal class packingVM : LifeCycleViewModelBase, IScanner
    {
        #region properties
        #endregion
        public packingVM()
        {
            string in_fn = Path.Combine(Directory.GetCurrentDirectory(), "test.docx");
            string out_fn = Path.Combine(Directory.GetCurrentDirectory(), "out.docx");

            Document doc = new Document(in_fn);
            DocumentBuilder builder = new DocumentBuilder(doc);
            Shape shape = (Shape)doc.GetChild(NodeType.Shape, 1, true);
            builder.MoveTo(shape);
            shape.Remove();
            Shape image = builder.InsertImage("out.png");
            doc.Save(out_fn);


        }

        #region override
        public void OnScan(string text)
        {            
        }
        #endregion
    }
}
