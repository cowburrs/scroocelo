using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class HexMap
{
    private int m_cols;
    private int m_rows;
    private int[,] m_hex_adj_map;
    private List<int> m_blocked;
    [Flags] public enum connection_type
    {
        even_row = 1,
        odd_row = 2,
        left_edge = 4,
        right_edge = 8,
        bottom_edge = 16,
        top_edge = 32
    }

    public int GetRow(int tile)
    {
            return tile / m_cols;
    } 
    public int GetColumn(int tile)
    {
            return tile % m_cols;
    } 

    public int GetTile(int row, int col)
    {
            return (row * m_cols) + col;
    }
    public connection_type ConnectionType(int tile)
    {
        connection_type c_type = 0;                        // Bit flag "connection type"
        int row = GetRow(tile);
        int col = GetColumn(tile);

        if (row % 2 == 0) 
        {
            c_type = c_type | connection_type.even_row;
        }
        if (row % 2 != 0)
        {
            c_type = c_type | connection_type.odd_row;
        }
        if (row == m_rows - 1) {
            c_type = c_type | connection_type.bottom_edge;
        }
        if (row == 0)
        {
            c_type = c_type | connection_type.top_edge;
        }
        if (col == m_cols - 1)
        {
            c_type = c_type | connection_type.right_edge;
        }
        if (col == 0)
        {
            c_type = c_type | connection_type.left_edge;
        }

        return c_type;
    }

    public void ConnectTiles(int tile1, int tile2)
    {
        if (!(m_blocked.Contains(tile1) || m_blocked.Contains(tile2)))
        {
            m_hex_adj_map[tile1, tile2] = 1;
        }
        else
        {
            m_hex_adj_map[tile1, tile2] = 0;
        }
    }

    public void Connect()
    {
        for (int i = 0; i < m_rows * m_cols; i++)
        {
            connection_type c_type = ConnectionType(i);
            int row = GetRow(i); 
            int col = GetColumn(i);

            if (c_type.HasFlag(connection_type.top_edge) && c_type.HasFlag(connection_type.left_edge))
            {
                ConnectTiles(i, GetTile(row, col + 1));
                ConnectTiles(i, GetTile(row + 1, col));
                ConnectTiles(i, GetTile(row + 1, col + 1));
            }
            else if (c_type.HasFlag(connection_type.top_edge) && c_type.HasFlag(connection_type.right_edge))
            {
                ConnectTiles(i, GetTile(row, col - 1));
                ConnectTiles(i, GetTile(row + 1, col));
                ConnectTiles(i, GetTile(row + 1, col - 1));
            }
            else if (c_type.HasFlag(connection_type.bottom_edge) && c_type.HasFlag(connection_type.left_edge))
            {
                ConnectTiles(i, GetTile(row, col + 1));
                ConnectTiles(i, GetTile(row - 1, col));
                ConnectTiles(i, GetTile(row - 1, col + 1));
            }
            else if (c_type.HasFlag(connection_type.bottom_edge) && c_type.HasFlag(connection_type.right_edge))
            {
                ConnectTiles(i, GetTile(row, col - 1));
                ConnectTiles(i, GetTile(row - 1, col));
                ConnectTiles(i, GetTile(row - 1, col - 1));
            }
            else if (c_type.HasFlag(connection_type.left_edge))
            {
                ConnectTiles(i, GetTile(row, col + 1));
                ConnectTiles(i, GetTile(row - 1, col));
                ConnectTiles(i, GetTile(row + 1, col));
            }
            else if (c_type.HasFlag(connection_type.right_edge))
            {
                ConnectTiles(i, GetTile(row, col - 1));
                ConnectTiles(i, GetTile(row - 1, col));
                ConnectTiles(i, GetTile(row + 1, col));
            }
            else if (c_type.HasFlag(connection_type.top_edge))
            {
                ConnectTiles(i, GetTile(row, col - 1));
                ConnectTiles(i, GetTile(row, col + 1));
                ConnectTiles(i, GetTile(row + 1, col));
            }
            else if (c_type.HasFlag(connection_type.bottom_edge))
            {
                ConnectTiles(i, GetTile(row, col - 1));
                ConnectTiles(i, GetTile(row, col + 1));
                ConnectTiles(i, GetTile(row - 1, col));
            }
            else
            {
                ConnectTiles(i, GetTile(row - 1, col - 1));
                ConnectTiles(i, GetTile(row - 1, col));
                ConnectTiles(i, GetTile(row, col - 1));
                ConnectTiles(i, GetTile(row, col + 1));
                ConnectTiles(i, GetTile(row + 1, col - 1));
                ConnectTiles(i, GetTile(row + 1, col));
            }
        }
    }
    public void Block(int tile)
    {
        m_blocked.Add(tile);
    }
    public HexMap(int rows, int max_width)
    {
        m_rows = rows;
        m_cols = max_width;
        m_hex_adj_map = new int[rows * max_width, rows * max_width];
        m_blocked = new List<int>();
        Connect();
    }

    public void Update()
    {
        Connect();
    }
    
    public void PrintHexAdjMap()
    {
        string adj_map = "";
        for (int i = 0; i < m_rows * m_cols; i++)
        {
            for (int j = 0; j < m_rows * m_cols; j++)
            {
                adj_map += m_hex_adj_map[i, j] + " ";
            }
            adj_map += '\n';
        }
        Debug.Print(adj_map);
    }
};
