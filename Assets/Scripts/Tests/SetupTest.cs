using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SetupTest
{
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("World1");

        yield return null;

        Assert.AreEqual(
            "World1",
            SceneManager.GetActiveScene().name);
    }

    [Test]
    public void Torches_Are_Configured_Correctly()
    {
        GameObject torches = GameObject.Find("Torches");

        Assert.IsNotNull(
            torches,
            "No GameObject named 'Torches' exists.");

        List<string> names = new();

        foreach (Transform child in torches.transform)
        {
            Assert.IsTrue(
                child.name.Contains("Torch"),
                $"'{child.name}' is inside Torches but does not contain 'Torch'.");

            names.Add(child.name);
        }

        var duplicates = names
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        Assert.IsEmpty(
            duplicates,
            $"Duplicate torch names found: {string.Join(", ", duplicates)}");
    }
}