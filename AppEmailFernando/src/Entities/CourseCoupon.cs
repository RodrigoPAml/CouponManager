using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AppEmailFernando.Entities
{
    /// <summary>
    /// Represents the input file used in this program.
    /// </summary>
    public class CourseCoupon
    {
        public ulong CourseId { get; set; }

        public string CourseName { get; set; }

        public ulong MaxRedemptions { get; set; }

        public ulong CouponCode { get; set; }

        public string CourseCuponUrl { get; set; }

        public bool Marked { get; set; }

        public static string Path => "input.csv";

        public static List<CourseCoupon> Open()
        {
            List<CourseCoupon> courseCoupons = new List<CourseCoupon>();

            using(StreamReader reader = File.OpenText(Path))
            {
                string line = string.Empty;
                uint currentRow = 1;

                while((line = reader.ReadLine()) != null)
                {
                    var columns = line.Split(';').ToList();

                    if(columns == null || columns.Count() != 5)
                        throw new Exception($"In row {currentRow}: Expected 5 columns but got {columns.Count()}!");

                    if(columns.Any(x => x == null || x.Count() == 0))
                        throw new Exception($"In row {currentRow}: All columns must have a value!");

                    if(currentRow == 1)
                    {
                        if(columns[0] != "course_id")
                            throw new Exception(@"Expected first column declaration to be ""course_id""");
                        if(columns[1] != "course_name")
                            throw new Exception(@"Expected second column declaration to be ""course_name""");
                        if(columns[2] != "maximum_redemptions")
                            throw new Exception(@"Expected third column declaration to be ""maximum_redemptions""");
                        if(columns[3] != "coupon_code")
                            throw new Exception(@"Expected fourth column declaration to be ""coupon_code""");
                        if(columns[4] != "course_coupon_url")
                            throw new Exception(@"Expected fifth column declaration to be ""course_coupon_url""");

                        currentRow++;

                        continue;
                    }

                    if(!ulong.TryParse(columns[0], out ulong id))
                        throw new Exception($@"In row {currentRow}: column ""course_id"" must be positive and integer!");

                    if(!ulong.TryParse(columns[2], out ulong maxRedemptions))
                        throw new Exception($@"In row {currentRow}: column ""max_redemptions must"" be positive and integer!");

                    if(!ulong.TryParse(columns[3], out ulong couponCode))
                        throw new Exception($@"In row {currentRow}: column ""coupon_code"" must be positive and integer!");

                    courseCoupons.Add(new CourseCoupon()
                    {
                        CourseId = id,
                        CourseName = columns[1],
                        MaxRedemptions = maxRedemptions,
                        CouponCode = couponCode,
                        CourseCuponUrl = columns[4],
                        Marked = false,
                    });

                    currentRow++;
                }

                if(courseCoupons.Count != courseCoupons.Select(x => x.CourseId).Distinct().Count())
                    throw new Exception(@"Value of column ""course_id"" is repeated!");

                return courseCoupons;
            }
        }

        public static void Save(List<CourseCoupon> courseCoupons)
        {
            int currentRow = 0;

            if(courseCoupons.Count != courseCoupons.Select(x => x.CourseId).Distinct().Count())
                throw new Exception(@"Value of column ""course_id"" is repeated!");

            using(StreamWriter writer = File.CreateText(Path))
            {
                writer.WriteLine("course_id;course_name;maximum_redemptions;coupon_code;course_coupon_url");

                foreach(var item in courseCoupons)
                {
                    string line = $"{item.CourseId};{item.CourseName};{item.MaxRedemptions};{item.CouponCode};{item.CourseCuponUrl}";

                    if(item.CourseName == null || item.CourseName.Count() == 0)
                        throw new Exception($@"In row {currentRow}: column ""course_name"" is empty!");

                    if(item.CourseCuponUrl == null || item.CourseCuponUrl.Count() == 0)
                        throw new Exception($@"In row {currentRow}: column ""course_cupon_url"" is empty!");

                    writer.WriteLine(line);

                    currentRow++;
                }
            }
        }
    }
}
