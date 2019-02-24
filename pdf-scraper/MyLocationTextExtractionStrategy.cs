
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace citycrime
{
    public class MyLocationTextExtractionStrategy : ITextExtractionStrategy, IRenderListener
    {
        public List<MyLocationTextExtractionStrategy.TextChunk> locationalResult = new List<MyLocationTextExtractionStrategy.TextChunk>();
        public static bool DUMP_STATE;

        public void BeginTextBlock()
        {
        }

        public void EndTextBlock()
        {
        }

        public string GetResultantText()
        {
            if (MyLocationTextExtractionStrategy.DUMP_STATE)
                this.DumpState();
            this.locationalResult.Sort();
            StringBuilder stringBuilder = new StringBuilder();
            MyLocationTextExtractionStrategy.TextChunk textChunk1 = (MyLocationTextExtractionStrategy.TextChunk)null;
            foreach (MyLocationTextExtractionStrategy.TextChunk textChunk2 in this.locationalResult)
            {
                if (textChunk1 == null)
                    stringBuilder.Append(textChunk2.text);
                else if (textChunk2.SameLine(textChunk1))
                {
                    float num = textChunk2.DistanceFromEndOf(textChunk1);
                    if ((double)num < -(double)textChunk2.charSpaceWidth)
                        stringBuilder.Append(' ');
                    else if ((double)num > (double)textChunk2.charSpaceWidth / 2.0 && (int)textChunk2.text[0] != 32 && (int)textChunk1.text[textChunk1.text.Length - 1] != 32)
                        stringBuilder.Append(' ');
                    stringBuilder.Append(textChunk2.text);
                }
                else
                {
                    stringBuilder.Append('\n');
                    stringBuilder.Append(textChunk2.text);
                }
                textChunk1 = textChunk2;
            }
            return stringBuilder.ToString();
        }

        private void DumpState()
        {
            foreach (MyLocationTextExtractionStrategy.TextChunk textChunk in this.locationalResult)
            {
                textChunk.PrintDiagnostics();
                Console.WriteLine();
            }
        }

        public void RenderText(TextRenderInfo renderInfo)
        {
            LineSegment baseline = renderInfo.GetBaseline();
            this.locationalResult.Add(new MyLocationTextExtractionStrategy.TextChunk(renderInfo.GetText(), baseline.GetStartPoint(), baseline.GetEndPoint(), renderInfo.GetSingleSpaceWidth()));
        }

        public void RenderImage(ImageRenderInfo renderInfo)
        {
        }

        public class TextChunk : IComparable<MyLocationTextExtractionStrategy.TextChunk>
        {
            internal string text;
            internal Vector startLocation;
            internal Vector endLocation;
            internal Vector orientationVector;
            internal int orientationMagnitude;
            internal int distPerpendicular;
            internal float distParallelStart;
            internal float distParallelEnd;
            internal float charSpaceWidth;

            public TextChunk(string str, Vector startLocation, Vector endLocation, float charSpaceWidth)
            {
                this.text = str;
                this.startLocation = startLocation;
                this.endLocation = endLocation;
                this.charSpaceWidth = charSpaceWidth;
                this.orientationVector = endLocation.Subtract(startLocation).Normalize();
                this.orientationMagnitude = (int)(Math.Atan2((double)this.orientationVector[1], (double)this.orientationVector[0]) * 1000.0);
                Vector v = new Vector(0.0f, 0.0f, 1f);
                this.distPerpendicular = (int)startLocation.Subtract(v).Cross(this.orientationVector)[2];
                this.distParallelStart = this.orientationVector.Dot(startLocation);
                this.distParallelEnd = this.orientationVector.Dot(endLocation);
            }

            public void PrintDiagnostics()
            {
                Console.WriteLine("Text (@" + (object)this.startLocation + " -> " + (string)(object)this.endLocation + "): " + this.text);
                Console.WriteLine("orientationMagnitude: " + (object)this.orientationMagnitude);
                Console.WriteLine("distPerpendicular: " + (object)this.distPerpendicular);
                Console.WriteLine("distParallel: " + (object)this.distParallelStart);
            }

            public bool SameLine(MyLocationTextExtractionStrategy.TextChunk a)
            {
                return this.orientationMagnitude == a.orientationMagnitude && this.distPerpendicular == a.distPerpendicular;
            }

            public float DistanceFromEndOf(MyLocationTextExtractionStrategy.TextChunk other)
            {
                return this.distParallelStart - other.distParallelEnd;
            }

            public int CompareTo(MyLocationTextExtractionStrategy.TextChunk rhs)
            {
                if (this == rhs)
                    return 0;
                int num1 = MyLocationTextExtractionStrategy.TextChunk.CompareInts(this.orientationMagnitude, rhs.orientationMagnitude);
                if (num1 != 0)
                    return num1;
                int num2 = MyLocationTextExtractionStrategy.TextChunk.CompareInts(this.distPerpendicular, rhs.distPerpendicular);
                if (num2 != 0)
                    return num2;
                return (double)this.distParallelStart < (double)rhs.distParallelStart ? -1 : 1;
            }

            private static int CompareInts(int int1, int int2)
            {
                if (int1 == int2)
                    return 0;
                return int1 >= int2 ? 1 : -1;
            }
        }
    }
}
