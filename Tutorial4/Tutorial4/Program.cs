namespace Tutorial3;

public class Program
    {
        public static void Main(string[] args)
        {
            var emps = Database.GetEmps();
            var depts = Database.GetDepts();
            var grades = Database.GetSalgrades();

            Console.WriteLine("1. Salesmen:");
            var salesmen = emps.Where(e => e.Job == "SALESMAN").ToList();
            salesmen.ForEach(e => Console.WriteLine(e.EName));

            Console.WriteLine("\n2. Dept 30 ordered by salary DESC:");
            var dept30 = emps.Where(e => e.DeptNo == 30).OrderByDescending(e => e.Sal).ToList();
            dept30.ForEach(e => Console.WriteLine($"{e.EName}: {e.Sal}"));

            Console.WriteLine("\n3. Employees from Chicago:");
            var chicagoDeptNos = depts.Where(d => d.Loc == "CHICAGO").Select(d => d.DeptNo).ToList();
            var fromChicago = emps.Where(e => chicagoDeptNos.Contains(e.DeptNo)).ToList();
            fromChicago.ForEach(e => Console.WriteLine(e.EName));

            Console.WriteLine("\n4. Names and salaries:");
            emps.Select(e => new { e.EName, e.Sal }).ToList()
                .ForEach(e => Console.WriteLine($"{e.EName}: {e.Sal}"));

            Console.WriteLine("\n5. Emp + Dept join:");
            var empDept = emps.Join(depts, e => e.DeptNo, d => d.DeptNo,
                (e, d) => new { e.EName, d.DName }).ToList();
            empDept.ForEach(x => Console.WriteLine($"{x.EName} - {x.DName}"));

            Console.WriteLine("\n6. Count per department:");
            var countPerDept = emps.GroupBy(e => e.DeptNo)
                .Select(g => new { DeptNo = g.Key, Count = g.Count() }).ToList();
            countPerDept.ForEach(x => Console.WriteLine($"Dept {x.DeptNo}: {x.Count}"));

            Console.WriteLine("\n7. Employees with commission:");
            emps.Where(e => e.Comm.HasValue)
                .Select(e => new { e.EName, e.Comm })
                .ToList()
                .ForEach(x => Console.WriteLine($"{x.EName}: {x.Comm}"));

            Console.WriteLine("\n8. Salary grades:");
            var empWithGrades = emps
                .SelectMany(e => grades
                    .Where(g => e.Sal >= g.Losal && e.Sal <= g.Hisal)
                    .Select(g => new { e.EName, g.Grade }))
                .ToList();
            empWithGrades.ForEach(x => Console.WriteLine($"{x.EName} - Grade {x.Grade}"));

            Console.WriteLine("\n9. Avg salary per department:");
            var avgPerDept = emps.GroupBy(e => e.DeptNo)
                .Select(g => new { DeptNo = g.Key, AvgSal = g.Average(e => e.Sal) }).ToList();
            avgPerDept.ForEach(x => Console.WriteLine($"Dept {x.DeptNo}: Avg {x.AvgSal}"));

            Console.WriteLine("\n10. Earning more than dept average:");
            var richerThanAvg = emps
                .Where(e => e.Sal > emps
                    .Where(x => x.DeptNo == e.DeptNo)
                    .Average(x => x.Sal))
                .ToList();
            richerThanAvg.ForEach(e => Console.WriteLine(e.EName));

            Console.WriteLine("\n11. Max salary:");
            Console.WriteLine(emps.Max(e => e.Sal));

            Console.WriteLine("\n12. Min salary in dept 30:");
            Console.WriteLine(emps.Where(e => e.DeptNo == 30).Min(e => e.Sal));

            Console.WriteLine("\n13. First 2 by hire date:");
            emps.OrderBy(e => e.HireDate).Take(2)
                .ToList().ForEach(e => Console.WriteLine($"{e.EName}: {e.HireDate:yyyy-MM-dd}"));

            Console.WriteLine("\n14. Distinct job titles:");
            emps.Select(e => e.Job).Distinct()
                .ToList().ForEach(j => Console.WriteLine(j));

            Console.WriteLine("\n15. Employees with managers:");
            emps.Where(e => e.Mgr.HasValue)
                .ToList().ForEach(e => Console.WriteLine($"{e.EName} has manager {e.Mgr}"));

            Console.WriteLine("\n16. All employees earn more than 500:");
            Console.WriteLine(emps.All(e => e.Sal > 500));

            Console.WriteLine("\n17. Any commission > 400:");
            Console.WriteLine(emps.Any(e => e.Comm.HasValue && e.Comm > 400));

            Console.WriteLine("\n18. Employee-Manager pairs:");
            var empMgrPairs = emps.Join(emps,
                e1 => e1.Mgr,
                e2 => e2.EmpNo,
                (e1, e2) => new { Employee = e1.EName, Manager = e2.EName }).ToList();
            empMgrPairs.ForEach(x => Console.WriteLine($"{x.Employee} → {x.Manager}"));

            Console.WriteLine("\n19. Income (Sal + Comm):");
            emps.Select(e => new { e.EName, Total = e.Sal + (e.Comm ?? 0) })
                .ToList().ForEach(x => Console.WriteLine($"{x.EName}: {x.Total}"));

            Console.WriteLine("\n20. Emp + Dept + Salgrade:");
            var fullJoin = emps
                .Join(depts, e => e.DeptNo, d => d.DeptNo, (e, d) => new { e, d })
                .SelectMany(
                    ed => grades
                        .Where(g => ed.e.Sal >= g.Losal && ed.e.Sal <= g.Hisal)
                        .Select(g => new { ed.e.EName, ed.d.DName, g.Grade }))
                .ToList();
            fullJoin.ForEach(x => Console.WriteLine($"{x.EName} - {x.DName} - Grade {x.Grade}"));

            Console.WriteLine("\n--- Finished ---");
        }
    }