// C# program to calculate solutions of linear
// equations using cramer's rule
using System;
using System.Collections.Generic;

public class CramersRule
{

// This functions finds the determinant of Matrix
static double determinantOfMatrix(double [,]mat)
{
    double ans;
    ans = mat[0,0] * (mat[1,1] * mat[2,2] - mat[2,1] * mat[1,2])
        - mat[0,1] * (mat[1,0] * mat[2,2] - mat[1,2] * mat[2,0])
        + mat[0,2] * (mat[1,0] * mat[2,1] - mat[1,1] * mat[2,0]);
    return ans;
}

// This function finds the solution of system of
// linear equations using cramer's rule
static List<double> findSolution(double [,]coeff)
{
    List<double> coeffs = new List<double>();
    // Matrix d using coeff as given in cramer's rule
    double [,]d = {
        { coeff[0,0], coeff[0,1], coeff[0,2] },
        { coeff[1,0], coeff[1,1], coeff[1,2] },
        { coeff[2,0], coeff[2,1], coeff[2,2] },
    };
    
    // Matrix d1 using coeff as given in cramer's rule
    double [,]d1 = {
        { coeff[0,3], coeff[0,1], coeff[0,2] },
        { coeff[1,3], coeff[1,1], coeff[1,2] },
        { coeff[2,3], coeff[2,1], coeff[2,2] },
    };
    
    // Matrix d2 using coeff as given in cramer's rule
    double [,]d2 = {
        { coeff[0,0], coeff[0,3], coeff[0,2] },
        { coeff[1,0], coeff[1,3], coeff[1,2] },
        { coeff[2,0], coeff[2,3], coeff[2,2] },
    };
    
    // Matrix d3 using coeff as given in cramer's rule
    double [,]d3 = {
        { coeff[0,0], coeff[0,1], coeff[0,3] },
        { coeff[1,0], coeff[1,1], coeff[1,3] },
        { coeff[2,0], coeff[2,1], coeff[2,3] },
    };

    // Calculating Determinant of Matrices d, d1, d2, d3
    double D = determinantOfMatrix(d);
    double D1 = determinantOfMatrix(d1);
    double D2 = determinantOfMatrix(d2);
    double D3 = determinantOfMatrix(d3);
    Console.Write("D is : {0:F6} \n", D);
    Console.Write("D1 is : {0:F6} \n", D1);
    Console.Write("D2 is : {0:F6} \n", D2);
    Console.Write("D3 is : {0:F6} \n", D3);

    // Case 1
    if (D != 0) 
    {
        // Coeff have a unique solution. Apply Cramer's Rule
        double x = D1 / D;
        double y = D2 / D;
        double z = D3 / D; // calculating z using cramer's rule
        Console.Write("Value of x is : {0:F6}\n", x);
        Console.Write("Value of y is : {0:F6}\n", y);
        Console.Write("Value of z is : {0:F6}\n", z);

        coeffs.Add(x);
        coeffs.Add(y);
        coeffs.Add(z);
        return coeffs;
    }
    
    // Case 2
    else
    {
        if (D1 == 0 && D2 == 0 && D3 == 0)
            Console.Write("Infinite solutions\n");
        else if (D1 != 0 || D2 != 0 || D3 != 0)
            Console.Write("No solutions\n");

    return coeffs;
    }
}

// Driver Code
public List<double> Main(List<double> numbers)
{
    // storing coefficients of linear
    // equations in coeff matrix
    double [,]coeff = {{ numbers[0], numbers[1], numbers[2], numbers[3] },
                        { numbers[4], numbers[5], numbers[6], numbers[7] },
                        { numbers[8], numbers[9], numbers[10], numbers[11] }};
    return findSolution(coeff);
    }
} 

// This code is contributed by 29AjayKumar
